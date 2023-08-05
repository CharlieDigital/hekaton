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
  /// The collector to use for storing the results.
  /// </summary>
  public Collector Collector { get; set; }

  /// <summary>
  /// The renderer to use to publish the results.
  /// </summary>
  public Renderer Renderer { get; set;}

  /// <summary>
  /// The list of scenarios described in the manifest for this test.
  /// </summary>
  public List<Scenario> Scenarios { get; set; } = new List<Scenario>();
}

/// <summary>
/// The strategy to use to store the results.
/// </summary>
public enum Collector {
  InMemory,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  Postgres,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  SqlLite,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  Dynamo,
}

/// <summary>
/// The strategy to use to render the results.
/// </summary>
public enum Renderer {
  Console,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  Html,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  Json,

  /// <summary>
  /// NOT IMPLEMENTED
  /// </summary>
  Markdown
}