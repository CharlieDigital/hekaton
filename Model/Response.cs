namespace Hekaton.Models;

public class Response {

  public Dictionary<string, string> Headers { get; set; } = new();

  public Dictionary<string, string> Cookies { get; set; } = new();
}