namespace Hekaton.Models;

public class Step {
  public string Name { get; set; } = "";

  public string Type { get; set; } = "";

  public Rows? Rows { get; set; }

  public string Url { get; set; } = "";

  public Dictionary<string, string> Headers { get; set; } = new();

  public string? Body { get; set; }

  public Pause? Pause { get; set; }

  public Response? Response { get; set; }

  public string? Generates { get; set; }
}