
using MathNet.Numerics.Distributions;

namespace Hekaton.Utility;

/// <summary>
/// Represents a duration string like "3s" and provides methods
/// to parse the value as TimeSpans
/// </summary>
public static class DurationString {
  /// <summary>
  /// Parses a duration as a TimeSpan.  For example "3s"  If no unit is specified,
  /// assume seconds.
  /// </summary>
  /// <param name="duration">The duration string like "3s" or "3m"</param>
  /// <returns>A TimeSpan that represents the duration.</returns>
  public static TimeSpan Parse(string? duration) {
    return Parse(duration, 0);
  }

  /// <summary>
  /// Parses a duration as a TimeSpan.  For example "3s"  If no unit is specified,
  /// assume seconds.  The second parameter defines the standard deviation to apply
  /// to the value to randomize the result.
  /// </summary>
  /// <param name="duration">The duration string like "3s" or "3m"</param>
  /// <param name="variation">A value that represents the variation as a percentage.</param>
  /// <returns>A TimeSpan that represents the duration.</returns>
  public static TimeSpan Parse(
    string? duration,
    decimal variation
  ) {
    if (duration == null) {
      return TimeSpan.FromSeconds(0);
    }

    var parsed = new char[duration.Length + 1];
    var split = 0;

    for (var i = 0; i < duration.Length; i++) {
      var c = duration[i];

      if (split == 0 && char.IsLetter(c)) {
        parsed[i] = '_';
        split = 1;
      }

      parsed[i + split] = c;
    }

    var parts = new string(parsed).Split('_');
    var time = int.Parse(parts[0]);
    var unit = parts[1].ToLowerInvariant();

    var result = unit switch {
      "s" => TimeSpan.FromSeconds(time),
      "ms" => TimeSpan.FromMilliseconds(time),
      "m" => TimeSpan.FromMinutes(time),
      "h" => TimeSpan.FromHours(time),
      _ => TimeSpan.FromSeconds(time)
    };

    // TODO: Cache calculated result?

    if (variation == 0) {
      return result;
    }

    // If the variation is non-zero, we calculate a the standard deviation using the
    // variation as a percentage.  We'll normalize to milliseconds.  For example,
    // 0.15 * 1000 milliseconds
    var stDev = (double) variation * result.TotalMilliseconds;

    var normal = new Normal(result.TotalMilliseconds, stDev, Random.Shared);

    return TimeSpan.FromMilliseconds(normal.Sample());
  }
}
