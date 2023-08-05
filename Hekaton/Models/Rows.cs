namespace Hekaton.Models;

/// <summary>
/// Defines the origin for a dataset that VUsers can consume.  For example, rows
/// of username,password,email combinations.
/// </summary>
public class Rows {
  /// <summary>
  /// The path to the source of the input file (e.g. CSV)
  /// </summary>
  public string Source { get; set; } = "";

  /// <summary>
  /// How the rows should be read.
  /// </summary>

  public RowReadStyle Read { get; set; }

  /// <summary>
  /// The mapping of columns to variables (in order of columns)
  /// </summary>

  public string[] Columns { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Defines how to read the rows.
/// </summary>
public enum RowReadStyle {
  /// <summary>
  /// Read the rows in-order.
  /// </summary>
  InOrder,

  /// <summary>
  /// Randomly read rows from the source.
  /// </summary>
  Random
}
