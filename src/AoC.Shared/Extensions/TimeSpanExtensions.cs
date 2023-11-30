namespace AoC.Shared.Extensions;

public static class TimeSpanExtensions
{
    public static string Format(this TimeSpan time)
    {
        var ns = time.TotalNanoseconds;
        if (ns < 1000)
            return $"{ns} ns";

        var ms = time.TotalMicroseconds;
        if (ms < 1000)
            return $"{ms} µs";
        
        ms = time.TotalMilliseconds;
        if (ms < 1000)
            return $"{ms} ms";

        if (time.TotalSeconds < 180)
            return time.TotalSeconds.ToString("##.### 's'");

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (time.TotalMinutes < 180)
            return time.TotalMinutes.ToString("##.## 'min'");

        return time.TotalHours.ToString("##.## 'h'");
    }
}
