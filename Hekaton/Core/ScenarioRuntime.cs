
using System.Diagnostics;
using System.Threading.Channels;

namespace Hekaton.Core;

/// <summary>
/// The scenario runtime scope.  Each instance represents a VUser.
/// </summary>
public class ScenarioRuntime {
  /// <summary>
  /// Initializes a new runtime instance.
  /// </summary>
  /// <param name="config">The configuration for this instance.</param>
  /// <param name="delay">
  /// The initial delay once the runtime is started.  This determines when the user
  /// comes online.  For example, a user may be activated 10 minutes into the test
  /// run.
  /// </param>
  /// <param name="identifier">A numeric identifier for this instance.</param>
  public ScenarioRuntime(
    Scenario config,
    TimeSpan delay,
    int identifier = 0
  ) {
    Config = config;
    Delay = delay;
    Identifier = identifier;
  }

  /// <summary>
  /// The definition for this
  /// </summary>
  private Scenario Config { get; }

  /// <summary>
  /// The delay assigned to this scenario runtime.  This will offset the start of
  /// the execution of this runtime instance by the specified amount.  Use this to
  /// simulate users ramping on at different times.
  /// </summary>
  public TimeSpan Delay { get; }

  /// <summary>
  /// The numeric identifier for this instance of the runtime.  The default is 0.
  /// </summary>
  public int Identifier { get; }


  /// <summary>
  /// Initializes the scenario runtime for the VUser.
  /// </summary>
  /// <param name="writer">The channel writer which allows us to send our results</param>
  public async Task InitAsync(ChannelWriter<ScenarioEvent> writer) {
    if (Delay.TotalNanoseconds > 0) {
      await Task.Delay(Delay);
    }

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    try {


      // TODO: Execute steps; placeholder delay to simulate
      await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(1000, 3000)));

      stopwatch.Stop();

      await writer.WriteAsync(new ScenarioCompletedEvent() {
        ScenarioName = Config.Name,
        Identifier = Identifier,
        Timing = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)
      });
    } catch (Exception ex) {
      stopwatch.Stop();

      await writer.WriteAsync(new ScenarioErrorEvent() {
        Exception = ex,
        Message = "",
        Timing = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)
      });
    }
  }
}
