namespace Hekaton.Models;

/// <summary>
/// Defines the strategy for ramping virtual users over time.  For example, we may
/// wish to add 20 users every 60 seconds.
/// </summary>
public class Ramp {
  /// <summary>
  /// The duration for each tick when the next set of virtual users will be added.
  /// A value like "10s" or every 10 seconds.
  /// </summary>
  public string Every { get; set; } = "";

  /// <summary>
  /// A variation to the duration on each tick expressed as a percentage of the
  /// value.
  /// </summary>
  public decimal Variation { get; set; } = 0;

  /// <summary>
  /// How many users to add on each tick.  For example, every 10 seconds, add 10
  /// users.  Combined with the VUser.Max, this defines the total number of users
  /// at the end of the test.
  /// </summary>
  public int Add { get; set; } = 1;
}