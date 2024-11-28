using System.Diagnostics;
using AoC.Shared;

namespace AoC._2023.Days;

public sealed class Day01 : DayBase<IEnumerable<string>, int, FileInputReader>
{
    protected override string Name => "Day 1: Trebuchet?!";

    protected override IEnumerable<string> ParseInput(IEnumerable<string> lines) => lines;

    protected override int RunPart1(IEnumerable<string> input)
    {
        return input.Where(l => l.Length > 0).Sum(l =>
        {
            var firstDigit = l.SkipWhile(c => !char.IsDigit(c)).First() - '0';
            var lastDigit = l.Reverse().SkipWhile(c => !char.IsDigit(c)).First() - '0';
            return firstDigit * 10 + lastDigit;
        });
    }

    protected override int RunPart2(IEnumerable<string> input)
    {
        return input.Where(l => l.Length > 0).Sum(l =>
        {
            var firstDigit = FindFirstDigit(l, 0, 1);
            var lastDigit = FindFirstDigit(l, l.Length - 1, -1);
            return firstDigit * 10 + lastDigit;
        });
    }

    private static int FindFirstDigit(string line, int firstIndex, int nextIndexOffset)
    {
        var dict = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        for (var i = firstIndex; i >= 0 && i < line.Length; i += nextIndexOffset)
        {
            if (char.IsDigit(line[i]))
                return line[i] - '0';

            var part = line[i..];
            
            foreach (var (key, value) in dict)
            {
                if (part.StartsWith(key))
                    return value;
            }
        }

        throw new UnreachableException();
    }
}
