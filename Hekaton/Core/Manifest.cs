using Hekaton.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hekaton.Core;

/// <summary>
/// Represents a manifest that defines a set of test scenarios.  The manifest is a
/// wrapper around the underlying test definition.
/// </summary>
public class Manifest {
  private readonly Test _test;

  /// <summary>
  /// Private constructor; use one of the <c>Load</c> methods to create an instance.
  /// </summary>
  private Manifest(Test test) {
    _test = test;
  }

  /// <summary>
  /// Getter for the test loaded from the manifest.
  /// </summary>
  public Test Test => _test;

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
  public (bool isValid, IEnumerable<string> errors) Validate() {
    // TODO: Implement validation.
    return (true, Array.Empty<string>());
  }

  /// <summary>
  /// Prepares the test case in the manifest by creating the artifacts required to
  /// execute but does not start the execution.
  /// </summary>
  public Manifest Prepare() {



    return this;
  }
}