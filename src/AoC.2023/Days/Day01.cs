using AoC.Shared;
using MoreLinq.Extensions;

namespace AoC._2023.Days;

public sealed class Day01 : DayBase<IEnumerable<int>, int>
{
    protected override string Name => "Day 1: Calorie Counting";

    protected override IEnumerable<int> ParseInput(IEnumerable<string> lines)
        => lines.Select(l => string.IsNullOrEmpty(l) ? -1 : int.Parse(l))
            .Split(-1)
            .Select(c => c.Sum())
            .OrderDescending();

    protected override int RunPart1(IEnumerable<int> input)
        => input.First();
    
    protected override int RunPart2(IEnumerable<int> input)
        => input.Take(3).Sum();
}
