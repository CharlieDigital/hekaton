namespace Hekaton.Models;

/// <summary>
/// A step represents an HTTP action that occurs based on a user action in the
/// application.  For example, an API call, file download, or requesting an HTML
/// page.
/// </summary>
public class Step {
  /// <summary>
  /// A friendly name to identify the step.
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// The type of the step.
  /// </summary>
  public string Type { get; set; } = "";

  /// <summary>
  /// The URL action of the step.  This can be an absolute or relative URL.  If a
  /// base URL is specified for the test, it will be applied to relative URLs.
  /// </summary>
  public string Url { get; set; } = "";

  /// <summary>
  /// A dictionary which contains the headers to apply to the request.  This can
  /// include dynamically generated values which use variables captured from
  /// earlier responses.
  /// </summary>
  public Dictionary<string, string> Headers { get; set; } = new();

  /// <summary>
  /// For POST, PUT, and PATCH requests, the body of the request as a string.
  /// </summary>
  public string? Body { get; set; }

  /// <summary>
  /// When specified, applies a pause after this step executes before starting the
  /// next step.  This simulates how long the user would wait before starting the
  /// next action.  This will override the default value for the Scenario when
  /// specified.  If neither are specified, the next action executes immediately.
  /// </summary>
  public Pause? Pause { get; set; }

  /// <summary>
  /// Defines how to handle the response from the URL by mapping headers and
  /// cookies to variables that can be used in subsequent steps.
  /// </summary>
  public Response? Response { get; set; }

  /// <summary>
  /// To more accurately represent the step, you can optionally generate additional
  /// requests that would be caused by the action on this step.  For example, if
  /// the step is to download an HTML file, this could include all of the additional
  /// requests to grab images, static JS, and so on. This can more accurately reflect
  /// load when -- as an example -- the application my dynamically generate images.
  /// </summary>
  public string[]? Generates { get; set; }
}