namespace Hekaton.Models;

/// <summary>
/// Defines the strategy for creating virtual users.
/// </summary>
public class Vusers {
  /// <summary>
  /// The initial number of virtual users.
  /// </summary>
  public int Initial { get; set; } = 0;

  /// <summary>
  /// The target max number of virtual users.  If this value is less than or equal
  /// to the initial value, no virtual users are added.
  /// </summary>
  public int Max { get; set; } = -1;

  /// <summary>
  /// The strategy for ramping the virtual users over time.
  /// </summary>
  public Ramp? Ramp { get; set; }
}