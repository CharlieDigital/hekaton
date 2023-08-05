namespace Hekaton.Models;

/// <summary>
/// Defines a pause in user action that models activities like browsing, scrolling,
/// reading, and so on.
/// </summary>
public class Pause {
  /// <summary>
  /// A duration like "3s" for 3 seconds.
  /// </summary>
  public string Duration { get; set; } = "";

  /// <summary>
  /// The variance in the duration.  If not specified, the exact duration is used.
  /// This should be a decimal value that represents a percentage +/- of the
  /// duration value.
  /// </summary>
  public decimal Variation { get; set; } = 0;
}
