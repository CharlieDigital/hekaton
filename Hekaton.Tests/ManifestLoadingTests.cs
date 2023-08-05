using Hekaton.Core;
using Hekaton.Models;
using Hekaton.Utility;

namespace Hekaton.Tests;
public class ManifestLoadingTests {
  [Fact]
  public void Can_Deserialize_Yaml_Manifest() {
    var path = ResolveFullPath("sample.yaml");

    Console.WriteLine(path);

    var manifest = Manifest.LoadFromFile(path);

    Assert.NotNull(manifest);
    Assert.Equal("This is the name of the test", manifest.Test.Name);
  }

  [Fact]
  public void Can_Parse_Duration_Strings() {
    Assert.Equal(
      TimeSpan.FromMilliseconds(3),
      DurationString.Parse("3ms"));

    Assert.Equal(
      TimeSpan.FromSeconds(3),
      DurationString.Parse("3s"));

    Assert.Equal(
      TimeSpan.FromMinutes(3),
      DurationString.Parse("3m"));

    Assert.Equal(
      TimeSpan.FromHours(3),
      DurationString.Parse("3h"));
  }

  [Fact]
  public void Duration_With_Variation_Uses_Normal_Distribution() {
    // Variation of up to 1.5ms
    Assert.NotEqual(
      TimeSpan.FromMilliseconds(3),
      DurationString.Parse("3ms", 0.5m));

    // Variation of up to 1.5s.
    Assert.NotEqual(
      TimeSpan.FromSeconds(3),
      DurationString.Parse("3s", 0.5m));
  }

  private static string ResolveFullPath(string path) {
    return Path.Combine(Environment.CurrentDirectory, "Manifests", path);
  }
}
