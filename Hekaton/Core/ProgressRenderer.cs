using System.Threading.Channels;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Hekaton.Core;

/// <summary>
/// Wrapper around Spectre.Console UI console renderer.  The renderer will render
/// a row for each scenario and each step in the scenario.  The results are an
/// aggregate across all scenario runtime instances.
/// </summary>
public class ProgressRenderer {
  private readonly Test _test;
  private readonly Table _table;
  private readonly Channel<RowOperation> _channel;
  private readonly ChannelWriter<RowOperation> _writer;
  private readonly ChannelReader<RowOperation> _reader;
  private readonly Dictionary<string, (int Index, int Completed, int Errors)> _rowMap;
  private readonly Dictionary<string, (decimal mean, decimal p90, decimal p95)> _slaMap;

  public ProgressRenderer(Test test) {
    _channel = Channel.CreateUnbounded<RowOperation>();
    _writer = _channel.Writer;
    _reader = _channel.Reader;

    _rowMap = new();
    _slaMap = new();

    _table = new Table();
    _test = test;
  }

  /// <summary>
  /// True when this instance has been started.
  /// </summary>
  public bool Started { get; private set; }

  /// <summary>
  /// Starts the progress renderer with a while loop reading the output end of the
  /// channel.  Be sure to call stop.
  /// </summary>
  public async Task StartAsync() {
    Started = true;

    await AnsiConsole
      .Live(_table)
      .StartAsync(async ctx =>
      {
        Console.WriteLine(""); // Blank line
        _table.Title = new TableTitle(_test.Name);
        _table.AddColumn("Scenario", (c) => c.Width = 45);
        _table.AddColumn(new TableColumn("Completed").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("Errors").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("Total").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("Min").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("Mean").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("P90").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("P95").Alignment(Justify.Right));
        _table.AddColumn(new TableColumn("Max").Alignment(Justify.Right));

        // Build the initial table state
        var offset = 0;

        foreach (var scenario in _test.Scenarios) {
          var instanceCount = scenario switch {
            { Vusers: null } => 1,
            var cfg when cfg.Vusers.Max < cfg.Vusers.Initial => cfg.Vusers.Initial,
            var cfg => cfg.Vusers.Max
          };

          var scenarioKey = scenario.Name.Clean();
          _rowMap[scenarioKey] = (_rowMap.Count + offset, 0, 0);

          _table.AddRow(scenarioKey, "0", "0", instanceCount.ToString());

          Console.WriteLine($"Add scenario key {scenarioKey}");

          foreach (var step in scenario.Steps) {
            var stepKey = step.Name.Clean();
            var scenarioStepKey = $"{scenarioKey}.{stepKey}";

            _table.AddRow($"  ⮑  {stepKey}", "0", "0", instanceCount.ToString());

            Console.WriteLine($"Add step key {stepKey}");

            _rowMap[scenarioStepKey] = (_rowMap.Count + offset, 0, 0);

            if (step.Sla != null) {
              _slaMap[scenarioStepKey] = (step.Sla.Mean, step.Sla.P90, step.Sla.P95);
            }
          }

          _table.AddEmptyRow();
          offset++;
        }

        ctx.Refresh();

        // This is the read loop which will consume from the writer.
        while (await _reader.WaitToReadAsync()) {
          if (_reader.TryRead(out var o)) {
            var (_, index, completed, mean, p90, p95, errors, min, max) = o;

            _table.UpdateCell(index, 1, completed);
            _table.UpdateCell(index, 2, errors);
            _table.UpdateCell(index, 4, min);
            _table.UpdateCell(index, 5, mean);
            _table.UpdateCell(index, 6, p90);
            _table.UpdateCell(index, 7, p95);
            _table.UpdateCell(index, 8, max);

            ctx.Refresh();
          }
        }
      });
  }

  /// <summary>
  /// Signals to the reader that all writing is completed.
  /// </summary>
  public void Stop() {
    _writer.Complete();
  }

  public async Task UpdateScenarioAsync(
    string key
  ) {
    var (index, completed, errors) = _rowMap[key];
    _rowMap[key] = (index, completed += 1, errors);

    await _writer.WriteAsync(new(
      key,
      index,
      completed.ToString(),
      Text.Empty,
      Text.Empty,
      Text.Empty
    ));
  }

  /// <summary>
  /// Updates the statistics on a given step.  All values are expressed as milliseconds
  /// </summary>
  /// <param name="scenarioKey">The cleaned name of the scenario.</param>
  /// <param name="stepKey">The cleaned name of the step</param>
  /// <param name="mean">The mean.</param>
  /// <param name="p90">The P90.</param>
  /// <param name="p95">The P95.</param>
  /// <param name="min">The minimum value.</param>
  /// <param name="max">The maximum value.</param>
  public async Task UpdateStepAsync(
    string scenarioKey,
    string stepKey,
    double mean,
    double p90,
    double p95,
    double min,
    double max
  ) {
    var key = $"{scenarioKey}.{stepKey}";
    var (index, completed, errors) = _rowMap[key];
    _rowMap[key] = (index, completed += 1, errors);

    var meanRendered = Markup.FromInterpolated($"[green]{mean:F1}[/]");
    var p90Rendered = Markup.FromInterpolated($"[green]{p90:F1}[/]");
    var p95Rendered = Markup.FromInterpolated($"[green]{p95:F1}[/]");

    if (_slaMap.ContainsKey(key)) {
      var (meanSla, p90Sla, p95Sla) = _slaMap[key];

      if (meanSla > 0 && mean > decimal.ToDouble(meanSla)) {
        meanRendered = Markup.FromInterpolated($"[red]{mean:F1}[/]");
      }

      if (p90Sla > 0 && p90 > decimal.ToDouble(p90Sla)) {
        p90Rendered = Markup.FromInterpolated($"[red]{p90:F1}[/]");
      }

      if (p95Sla > 0 && p95 > decimal.ToDouble(p95Sla)) {
        p95Rendered = Markup.FromInterpolated($"[red]{p95:F1}[/]");
      }
    }

    await _writer.WriteAsync(new(
      key,
      index,
      completed.ToString(),
      meanRendered,
      p90Rendered,
      p95Rendered,
      "0",
      min.ToString("F1"),
      max.ToString("F1")
    ));
  }

  public record RowOperation(
    string Key,
    int Index,
    string Completed,
    IRenderable Mean,
    IRenderable P90,
    IRenderable P95,
    string Errors = "",
    string Min = "",
    string Max = ""
  );
}