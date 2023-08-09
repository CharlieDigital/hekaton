
using CommandLine;
using CommandLine.Text;
using Spectre.Console;

var log = (object message) => Console.WriteLine(message);

// See docs here for CommandLineParser: https://github.com/commandlineparser/commandline/wiki
var parser = new Parser(with => with.EnableDashDash = true);
var result = parser.ParseArguments<RuntimeOptions>(args);

var exec = async Task (RuntimeOptions options) => {
  var file = Path.Combine(Environment.CurrentDirectory, options.FileName);
  var manifest = Manifest.LoadFromFile(file);

  if (manifest == null) {
    log($"[ERROR] The file '{file}' could not be located.");

    return;
  }

  log("Preparing the manifest...");

  var duration = await manifest.Prepare().RunAsync();

  AnsiConsole.MarkupLineInterpolated($"[green] ⧖ Total duration: {duration}[/]");
};

await result
  .WithNotParsed(errors => DisplayHelp(result))
  .WithParsedAsync((options) => exec(options));

static void DisplayHelp<T>(ParserResult<T> result)
{
  var helpText = HelpText.AutoBuild(result, h =>
  {
    h.AdditionalNewLineAfterOption = false;
    h.Heading = "Hekaton Web Performance Test Tool";
    h.Copyright = "Copyright (c) 2023 Charles Chen";
    return HelpText.DefaultParsingErrorsHandler(result, h);
  }, e => e);
  Console.WriteLine(helpText);
}