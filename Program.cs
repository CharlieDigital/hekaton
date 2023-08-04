using Hekaton.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var path = Path.Combine(Environment.CurrentDirectory, "sample.yaml");
var yaml = System.IO.File.ReadAllText(path);

var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml
    .Build();

var test = deserializer.Deserialize<Test>(yaml);

Console.WriteLine(test.Name);

Console.WriteLine(test.Scenarios[1]?.Steps[0]?.Response?.Headers["__auth"] ?? "");