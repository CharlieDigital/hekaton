namespace Hekaton.Models;

/// <summary>
/// Used to model the mapping of the response from the HTTP request to variables
/// that can be retrieved in later steps.  For example, define capture of the
/// Authentication header as the variable __auth.
/// </summary>
public class Response {

  /// <summary>
  /// A dictionary that contains the mapping of dynamic variables extracted from
  /// the response headers.  The key is the name of the variable.  The value is the
  /// name of the header to extract the value from.
  /// </summary>
  public Dictionary<string, string> Headers { get; set; } = new();

  /// <summary>
  /// A dictionary that contains the mapping of dynamic variables extracted from
  /// the cookies.  The key is the name of the variable.  The value is the
  /// name of the cookie to extract the value from.  Use the special value __all
  /// to capture the entire set of cookies.
  /// </summary>
  public Dictionary<string, string> Cookies { get; set; } = new();
}
