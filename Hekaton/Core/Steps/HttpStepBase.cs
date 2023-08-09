using System.Net.Http;
using System.Net.Http.Headers;

namespace Hekaton.Core.Steps;

/// <summary>
/// Base class for HttpSteps which encapsulates core functionality for interfacing
/// with HTTP endpoints.
/// </summary>
public abstract class HttpStepBase {
  protected HttpStepBase(Scenario scenario, Step config) {
    Scenario = scenario;
    Config = config;
  }

  protected Scenario Scenario { get; }

  protected Step Config { get; }

  /// <summary>
  /// Execute the step using the supplied HttpClient and an optional pause from the
  /// scenario that contains the step.  If the step config has its own pause, it
  /// will use the step config pause.
  /// </summary>
  /// <param name="httpClient">The HttpClient to use for the action.</param>
  public async Task<IEnumerable<(string Key, string Value)>> ExecuteAsync(
    HttpClient httpClient
  ) {
    // Prepare common request actions like setting the headers.

    var output = new List<(string, string)>();

    var message = new HttpRequestMessage(Method, Config.Url);

    foreach (var header in Config.Headers) {
      // TODO Perform string replacement on the headers.
      message.Headers.Add(header.Key, header.Value);
    }

    PrepareMessage(message);

    var response = await httpClient.SendAsync(message);

    if (Config.Response != null) {
      output.AddRange(ExtractResponseValues(response.Headers));
    }

    // If there is a pause for this step, we execute it.
    var pause = Config.Pause ?? Scenario.Pause;

    if (pause != null) {
      await pause.NowAsync();
    }

    return output;
  }

  protected abstract HttpMethod Method { get; }

  protected virtual void PrepareMessage(HttpRequestMessage message) {

  }

  private IEnumerable<(string, string)> ExtractResponseValues(HttpResponseHeaders headers) {
    throw new NotImplementedException();
  }
}