namespace Hekaton.Models;

public class Test {
  public string Name { get; set; } = "";

  public string BaseUrl { get; set; } = "";

  public List<Scenario> Scenarios { get; set; } = new List<Scenario>();
}