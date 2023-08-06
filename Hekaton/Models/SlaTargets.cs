namespace Hekaton.Models;

/// <summary>
/// Models the SLA targets for a given step.
/// </summary>
public class SlaTargets {
  /// <summary>
  /// When non-zero, the mean for this request.
  /// </summary>
  public decimal Mean { get; set; }

  /// <summary>
  /// When non-zero, the P90 target for this request.
  /// </summary>
  public decimal P90 { get; set; }

  /// <summary>
  /// When non-zero, the P95 target for this request.
  /// </summary>
  public decimal P95 { get; set; }
}