using Hekaton.Core;
using Hekaton.Models;

namespace Hekaton.Tests;
public class ScenarioResolutionTests {

  [Fact]
  public void Resolves_Single_Scenario_Runtime() {
    var scenario = new Scenario() {
      Name = "Test",
    };

    var result = Manifest.ResolveScenarioRuntimes(scenario).ToArray();

    Assert.Single(result);
  }

  [Fact]
  public void Resolves_Initial_Scenario_Runtimes() {
    var scenario = new Scenario() {
      Name = "Test",
      Vusers = new() {
        Initial = 5
      }
    };

    var result = Manifest.ResolveScenarioRuntimes(scenario).ToArray();

    Assert.Equal(5, result.Length);
    Assert.True(result.All(r => r.Delay.TotalMilliseconds == 0));
  }

  [Fact]
  public void Resolves_Scenario_Runtimes_With_Ramp_No_Variation() {
    // dotnet test --filter "FullyQualifiedName~Resolves_Scenario_Runtimes_With_Ramp_No_Variation"
    var scenario = new Scenario() {
      Name = "Test",
      Vusers = new() {
        Initial = 1,
        Max = 4,
        Ramp = new() {
          Every = "10s",
          Variation = 0,
          Add = 1
        }
      }
    };

    var result = Manifest.ResolveScenarioRuntimes(scenario).ToArray();

    Assert.Equal(4, result.Length);

    var (first, second, third, fourth) = result;

    // The first instance should have a delay of 0s
    Assert.Equal(0, first!.Delay.TotalSeconds);

    // The second instance should have a delay of 10s
    Assert.Equal(10, second!.Delay.TotalSeconds);

    // The third instance should have a start of 20s
    Assert.Equal(20, third!.Delay.TotalSeconds);

    // The fourth instance should have a start of 30s
    Assert.Equal(30, fourth!.Delay.TotalSeconds);
  }

  [Fact]
  public void Resolves_Scenario_Runtimes_With_Ramp_With_Variation() {
    // dotnet test --filter "FullyQualifiedName~Resolves_Scenario_Runtimes_With_Ramp_With_Variation"
    var scenario = new Scenario() {
      Name = "Test",
      Vusers = new() {
        Initial = 1,
        Max = 3,
        Ramp = new() {
          Every = "10s",
          Variation = 0.5m, // Added variations
          Add = 1
        }
      }
    };

    var result = Manifest.ResolveScenarioRuntimes(scenario).ToArray();

    Assert.Equal(3, result.Length);

    var (_, second, third) = result;

    // The second instance will have a delay, but not exactly 10s
    Assert.NotEqual(10, second!.Delay.TotalSeconds);
    Assert.True(second!.Delay.TotalSeconds > 0);

    // The third instance will have a delay, but not exactly 20s
    Assert.NotEqual(20, third!.Delay.TotalSeconds);
    Assert.True(third!.Delay.TotalSeconds > 10);
  }
}
