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
  /// Loads a manifest using a fully qualified file path.
  /// </summary>
  /// <param name="path">A fully qualified file path.</param>
  /// <returns>An instance of the manifest.</returns>
  public static Manifest? LoadFromFile(string path) {
    if (!File.Exists(path)) {
      return null;
    }

    var yaml = File.ReadAllText(path);

    var deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml
        .Build();

    var test = deserializer.Deserialize<Test>(yaml);

    return new Manifest(test);
  }
}