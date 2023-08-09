namespace Hekaton.Core.Steps;

public class HttpGetStep : HttpStepBase {
  public HttpGetStep(Step config) : base(config) {
  }

  protected override HttpMethod Method => HttpMethod.Get;

  protected override void PrepareMessage(HttpRequestMessage message) {
    // Set the body of the message.
  }
}