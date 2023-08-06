using System.Text.RegularExpressions;

namespace Hekaton.Utility;

public static partial class StringExtensions {
  private static readonly Regex AlphaNumeric = AlphaNumericRegex();

  [GeneratedRegex("[^a-zA-Z0-9_]+", RegexOptions.Compiled)]
  private static partial Regex AlphaNumericRegex();

  public static string Clean(this string input) {
    return AlphaNumeric.Replace(input, "_").ToLowerInvariant();
  }
}