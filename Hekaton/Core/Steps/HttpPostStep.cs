namespace Hekaton.Core.Steps;

public class HttpPostStep : HttpStepBase {
  public HttpPostStep(Scenario scenario, Step config) : base(scenario, config) {
  }

  protected override HttpMethod Method => HttpMethod.Post;
}