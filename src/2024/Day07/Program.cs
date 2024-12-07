#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var equations = lines.Select(l => new CalibrationEquation(l)).ToArray();

var part1 = equations.Where(e => e.IsSolvable(false)).Sum(equation => equation.Result);
var part2 = equations.Where(e => e.IsSolvable(true)).Sum(equation => equation.Result);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

internal class CalibrationEquation
{
    public long Result { get; }
    public long[] Parts { get; }

    public CalibrationEquation(string line)
    {
        var mainParts = line.Split(':');
        var parts = mainParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        Result = long.Parse(mainParts[0]);
        Parts = parts.Select(long.Parse).ToArray();
    }

    public bool IsSolvable(bool part2)
    {
        bool SolveRecursively(long currentResult, int i)
        {
            if (i == Parts.Length)
                return currentResult == Result;

            if (currentResult > Result)
                return false;
            
            var nextPart = Parts[i];
            if (SolveRecursively(currentResult + nextPart, i + 1)
                || SolveRecursively(currentResult * nextPart, i + 1))
                return true;
            
            return part2 && SolveRecursively(currentResult.Concat(nextPart), i + 1);
        }

        return SolveRecursively(Parts[0], 1);
    }
}

internal static class LongExtensions
{
    public static long Concat(this long a, long b)
    {
        return long.Parse($"{a}{b}");
    }
}
