using Hekaton.Core;

namespace Hekaton.Tests;

public class ManifestLoadingTests
{
    [Fact]
    public void Can_Deserialize_Yaml_Manifest()
    {
        var path = ResolveFullPath("sample.yaml");

        Console.WriteLine(path);

        var manifest = Manifest.LoadFromFile(path);

        Assert.NotNull(manifest);
        Assert.Equal("This is the name of the test", manifest.Test.Name);
    }

    private static string ResolveFullPath(string path) =>
        Path.Combine(Environment.CurrentDirectory, "Manifests", path);
}