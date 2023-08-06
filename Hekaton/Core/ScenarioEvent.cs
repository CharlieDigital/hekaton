using System.Text.RegularExpressions;

namespace Hekaton.Core;

public abstract partial class ScenarioEvent {
  private static readonly Regex AlphaNumeric = AlphaNumericRegex();

  [GeneratedRegex("[^a-zA-Z0-9_]+", RegexOptions.Compiled)]
  private static partial Regex AlphaNumericRegex();

  private string _scenarioName = "";

  public string ScenarioName {
    get => _scenarioName;
    set => _scenarioName = Clean(value);
  }

  public int Identifier { get; set; }

  public TimeSpan Timing { get; set; } = TimeSpan.MinValue;

  /// <summary>
  /// Cleans an input string by converting it to an alpha-numeric only lowercase
  ///  string.
  /// </summary>
  /// <param name="input">The input string.</param>
  /// <returns>Replaces all non-alphanumeric characters with "_" and lower cases the
  /// string.</returns>
  protected string Clean(string input) {
    return AlphaNumeric.Replace(input, "_").ToLowerInvariant();
  }
}

public class ScenarioCompletedEvent : ScenarioEvent {

}

public class ScenarioStartEvent : ScenarioEvent {
  public int StepCount { get; set; }
}

public class ScenarioStepEvent : ScenarioEvent {
  private string _stepName = "";

  public string StepName {
    get => _stepName;
    set => _stepName = Clean(value);
  }

  public string Url { get; set; } = "";

  public string Key => $"{ScenarioName}.{Identifier}.{StepName}";
}

public class ScenarioErrorEvent : ScenarioEvent {
  public Exception? Exception { get; set; }

  public string Message { get; set; } = "";
}
