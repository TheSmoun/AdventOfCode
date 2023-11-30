using System.Diagnostics;

namespace AoC.Shared.Extensions;

public static class FunctionExtensions
{
    public static (TResult, TimeSpan) MeasureWith<TInput, TResult>(this Func<TInput, TResult> function, TInput input)
    {
        var start = Stopwatch.GetTimestamp();
        var result = function(input);
        var elapsed = Stopwatch.GetElapsedTime(start);

        return (result, elapsed);
    }
}
