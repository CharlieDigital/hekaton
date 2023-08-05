namespace Hekaton.Core;

public class ScenarioEvent {
  public string ScenarioName { get; set; } = "";

  public int Identifier { get; set; }

  public TimeSpan Timing { get; set; } = TimeSpan.MinValue;
}

public class ScenarioStartEvent : ScenarioEvent {
  public int StepCount { get; set; }
}

public class ScenarioStepEvent : ScenarioEvent {
  public string StepName { get; set; } = "";

  public string Url { get; set; } = "";
}

public class ScenarioErrorEvent : ScenarioEvent {
  public Exception? Exception { get; set; }

  public string Message { get; set; } = "";
}
