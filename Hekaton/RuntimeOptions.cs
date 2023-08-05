using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace Hekaton;

/// <summary>
/// The runtime command line options.
///
/// See: https://github.com/commandlineparser/commandline/wiki
/// </summary>
public class RuntimeOptions {
  [NotNull]
  [Option('f', "filename", Required = true, HelpText = "Name of manifest YAML file.")]
  public string FileName { get; set; }
}