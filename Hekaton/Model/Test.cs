namespace Hekaton.Models;

/// <summary>
/// The root definition of a performance test.
/// </summary>
public class Test {
  /// <summary>
  /// A friendly name to identify the test.
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// An optional base URL to set for the test.
  /// </summary>
  public string BaseUrl { get; set; } = "";

  /// <summary>
  /// The list of scenarios described in the manifest for this test.
  /// </summary>
  public List<Scenario> Scenarios { get; set; } = new List<Scenario>();
}