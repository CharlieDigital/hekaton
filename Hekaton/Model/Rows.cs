namespace Hekaton.Models;

public class Rows {
  public string Source { get; set; } = "";

  public RowReadStyle Read { get; set; }

  public string[] Columns { get; set; } = Array.Empty<string>();
}

public enum RowReadStyle {
  InOrder,

  Random
}