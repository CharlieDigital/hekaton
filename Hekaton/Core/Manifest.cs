using System.Diagnostics;
using System.Threading.Channels;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hekaton.Core;
/// <summary>
/// Represents a manifest that defines a set of test scenarios.  The manifest is a
/// wrapper around the underlying test definition.
/// </summary>
public class Manifest {
  /// <summary>
  /// Private constructor; use one of the <c>Load</c> methods to create an instance.
  /// </summary>
  private Manifest(Test test) {
    Test = test;
  }

  /// <summary>
  /// Getter for the test loaded from the manifest.
  /// </summary>
  public Test Test { get; }

  /// <summary>
  /// The scenario runtimes for this manifest which are resolved by hydrating the
  /// virtual user instances from the scenario definition.
  /// </summary>
  public List<ScenarioRuntime> ScenarioRuntimes { get; } = new();

  /// <summary>
  /// The list of tasks that are started for the runtimes.
  /// </summary>
  public List<Task> ScenarioTasks { get; } = new();

  /// <summary>
  /// Loads a manifest using a fully qualified file path.  This pattern will allow
  /// the possibility of loading the YAML from different origins in the future (e.g.
  /// S3 bucket)
  /// </summary>
  /// <param name="path">A fully qualified file path.</param>
  /// <returns>An instance of the manifest.</returns>
  public static Manifest? LoadFromFile(string path) {
    if (!File.Exists(path)) {
      return null;
    }

    var yaml = File.ReadAllText(path);

    var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    var test = deserializer.Deserialize<Test>(yaml);

    return new Manifest(test);
  }

  /// <summary>
  /// Validates each part of the manifest.  For example, if a step uses a relative
  /// URL but no base URL is defined, this is a validation error.
  /// </summary>
  /// <returns>
  /// Returns a tuple that indicates whether the manifest is valid and specific
  /// errors which are encountered during validation.
  /// </returns>
  public static (bool isValid, IEnumerable<string> errors) Validate() {
    // TODO: Implement validation.
    return (true, Array.Empty<string>());
  }

  /// <summary>
  /// Prepares the test case in the manifest by creating the artifacts required to
  /// execute but does not start the execution.
  ///
  /// All of the scenarios instances are created at the outset; however they are all
  /// delayed according to their computed start time based on the delay, VUser, and
  /// ramp configurations.
  /// </summary>
  public Manifest Prepare() {
    foreach (var scenario in Test.Scenarios) {
      ScenarioRuntimes.AddRange(ResolveScenarioRuntimes(scenario));
    }

    return this;
  }

  /// <summary>
  /// Runs the test definition that is represented by the manifest.
  /// </summary>
  /// <returns>
  /// The duration of the test in milliseconds.
  /// </returns>
  public async Task<TimeSpan> RunAsync() {
    var channel = Channel.CreateUnbounded<ScenarioEvent>();
    var writer = channel.Writer;
    var reader = channel.Reader;

    var metrics = new MetricsCollector(Test.Collector, Test.Renderer);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    // Start the metrics collector so it is ready to consume events
    var metricsTask = metrics.StartAsync(reader);

    // Start the scenarios in parallel.
    Parallel.ForEach(ScenarioRuntimes,
      new() {
        MaxDegreeOfParallelism = Environment.ProcessorCount * 10
      },
      r => ScenarioTasks.Add(r.InitAsync(writer))
    );

    Console.WriteLine(ScenarioRuntimes.Count);
    Console.WriteLine(ScenarioTasks.Count);

    await Task
      .WhenAll(ScenarioTasks)
      .ContinueWith(_ => writer.Complete());

    stopwatch.Stop();

    await metricsTask;

    return TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
  }

  /// <summary>
  /// For a given scenario, resolve the runtime instances that represent the
  /// virtual users.
  /// </summary>
  /// <param name="scenario">The scenario definition.</param>
  /// <returns>A collection of runtime instances resolved from the definition.</returns>
  public static IEnumerable<ScenarioRuntime> ResolveScenarioRuntimes(Scenario scenario) {
    var scenarioDelay = DurationString.Parse(scenario.Delay);

    // Return a single runtime instance.
    if (scenario.Vusers == null) {
      yield return new(scenario, scenarioDelay);
      yield break; // !EXIT
    }

    // Return just the initial users.
    if (scenario.Vusers.Max <= scenario.Vusers.Initial
      || scenario.Vusers.Ramp == null) {
      for (var i = 0; i < scenario.Vusers.Initial; i++) {
        yield return new(scenario, scenarioDelay, i);
      }

      yield break; // !EXIT
    }

    // Use the VUsers definition to resolve a set of users.
    var rampDuration = DurationString.Parse(scenario.Vusers.Ramp.Every);
    var rampDelay = TimeSpan.FromSeconds(0);
    var rampThreshold = scenario.Vusers.Initial;

    // Use the VUsers definition to resolve a set of users.
    for (var i = 0; i < scenario.Vusers.Max; i++) {
      if (i < scenario.Vusers.Initial) {
        // The initial users get created using the scenario delay.
        yield return new(scenario, scenarioDelay, i);
      } else {
        // The subsequent set of users get added with the ramp strategy.
        if (i > rampThreshold) {
          rampDelay += rampDuration; // Increase the duration.
          rampThreshold += scenario.Vusers.Ramp.Add;
        }

        var computedDelay = scenarioDelay
          + rampDelay
          + DurationString.Parse(
              scenario.Vusers.Ramp.Every,
              scenario.Vusers.Ramp.Variation
            );

        yield return new(scenario, computedDelay, i);
      }
    }
  }
}
