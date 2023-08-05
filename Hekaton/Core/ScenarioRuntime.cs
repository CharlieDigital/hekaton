
namespace Hekaton.Core;

/// <summary>
/// The scenario runtime scope.  Each instance represents a VUser.
/// </summary>
public class ScenarioRuntime {
  /// <summary>
  /// Initializes a new runtime instance.
  /// </summary>
  /// <param name="delay">
  /// The initial delay once the runtime is started.  This determines when the user
  /// comes online.  For example, a user may be activated 10 minutes into the test
  /// run.
  /// </param>
  public ScenarioRuntime(TimeSpan delay) {
    Delay = delay;
  }

  /// <summary>
  /// The delay assigned to this scenario runtime.  This will offset the start of
  /// the execution of this runtime instance by the specified amount.  Use this to
  /// simulate users ramping on at different times.
  /// </summary>
  public TimeSpan Delay { get; }
}
