using System.Text.RegularExpressions;

namespace Hekaton.Core;

public abstract partial class ScenarioEvent {
  private string _scenarioName = "";

  public string ScenarioName {
    get => _scenarioName;
    set => _scenarioName = value.Clean();
  }

  public int Identifier { get; set; }

  public TimeSpan Timing { get; set; } = TimeSpan.MinValue;
}

public class ScenarioCompletedEvent : ScenarioEvent {

}

public class ScenarioStartEvent : ScenarioEvent {

}

public class ScenarioStepEvent : ScenarioEvent {
  private string _stepName = "";

  public string StepName {
    get => _stepName;
    set => _stepName = value.Clean();
  }

  public string Url { get; set; } = "";

  public string Key => $"{ScenarioName}.{Identifier}.{StepName}";
}

public class ScenarioErrorEvent : ScenarioEvent {
  public Exception? Exception { get; set; }

  public string Message { get; set; } = "";
}
