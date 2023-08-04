namespace Hekaton.Models;

/// <summary>
/// Models a user journey through the system with a series of steps that represent
/// HTTP actions that would be performed by the user.
/// </summary>
public class Scenario {
  /// <summary>
  /// A friendly name for identifying the scenario.
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// When present, specifies a delay to the start of the scenario.  For example,
  /// "3m" to delay the start of the scenario by 3 minutes.
  /// </summary>
  public string? Delay { get; set; }

  /// <summary>
  /// The configuration for the VUsers for this scenario.  The scenario will be
  /// executed once for each VUser.  If this value is not specified, the scenario
  /// is only executed once.
  /// </summary>
  public Vusers? Vusers { get; set; }

  /// <summary>
  /// When present, defines a set of rows to consume.  Each VUser will consume one
  /// row defined in the rows and use the values (e.g. username/password).
  /// </summary>
  public Rows? Rows { get; set; }

  /// <summary>
  /// Defines a default policy for each step to pause after completion.  This
  /// simulates human behavior such as browser, scrolling, reading, and so on.
  /// Each step can override this by defining a specific pause.
  /// </summary>
  public Pause? Pause { get; set; }

  /// <summary>
  /// The list of steps that are encapsulated by this scenario.
  /// </summary>
  public List<Step> Steps { get; set; } = new();
}