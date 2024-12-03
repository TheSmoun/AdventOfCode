using System.Text.RegularExpressions;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var singleLine = string.Join(string.Empty, lines);

var part1 = Utils.CalcResult(singleLine);
Console.WriteLine($"Part 1: {part1}");

var part2 = Utils.CalcResult(Utils.DisableMatcher().Replace(singleLine, string.Empty));
Console.WriteLine($"Part 2: {part2}");

internal static partial class Utils
{
    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    internal static partial Regex MulMatcher();

    [GeneratedRegex(@"don't\(\).*?do\(\)")]
    internal static partial Regex DisableMatcher();
    
    internal static int CalcResult(string instructions)
    {
        var matchCollection = MulMatcher().Matches(instructions);
        
        var sum = 0;
        for (var j = 0; j < matchCollection.Count; j++)
        {
            var match = matchCollection[j];
            var left = int.Parse(match.Groups[1].ValueSpan);
            var right = int.Parse(match.Groups[2].ValueSpan);
            sum += left * right;
        }
        
        return sum;
    }
}
