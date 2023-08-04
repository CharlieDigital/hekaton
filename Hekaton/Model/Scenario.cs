namespace Hekaton.Models;

public class Scenario {
  public string Name { get; set; } = "";

  public string? Delay { get; set; }

  public Vusers? Vusers { get; set; }

  public Rows? Rows { get; set; }

  public Pause? Pause { get; set; }

  public List<Step> Steps { get; set; } = new();
}