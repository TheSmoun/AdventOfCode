using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using AoC.Shared;

namespace AoC._2023.Days;

public partial class Day12 : DayBase<List<Day12.SpringGroup>, long>
{
    private static readonly Regex Regex = Unknowns();
    
    protected override string Name => "Day 12: Hot Springs";

    protected override List<SpringGroup> ParseInput(IEnumerable<string> lines)
    {
        return lines
            .Select(l =>
            {
                var parts = l.Split(' ');
                return new SpringGroup
                {
                    Configuration = parts[0],
                    ValidFormat = parts[1].Split(',').Select(int.Parse).ToArray(),
                };
            })
            .ToList();
    }

    protected override long RunPart1(List<SpringGroup> input)
    {
        return input.Sum(g =>
        {
            var hashPattern = g.ValidFormat.Select(i => "".PadRight(i, '#'));
            var regex = new Regex(@"^\.*" + string.Join(@"\.+", hashPattern) + @"\.*$");
            var possibleConfigurations = g.GetPossibleConfigurations();
            var count = possibleConfigurations.Count(c => regex.IsMatch(c));
            return count;
        });
    }

    protected override long RunPart2(List<SpringGroup> input)
    {
        return 0;
    }

    public class SpringGroup
    {
        public required string Configuration { get; init; }
        public required int[] ValidFormat { get; init; }

        public List<string> GetPossibleConfigurations()
        {
            var numberOfHashes = ValidFormat.Sum();
            var configurations = new List<string> { Configuration };
            var count = Configuration.Count(c => c == '?');
            
            for (var i = 0; i < count; i++)
            {
                var list = new List<string>();
                foreach (var configuration in configurations)
                {
                    if (MatchesWithReplacement(configuration, "#", numberOfHashes, out var c0))
                        list.Add(c0);
                    
                    if (MatchesWithReplacement(configuration, ".", numberOfHashes, out var c1))
                        list.Add(c1);
                }

                configurations = list;
            }

            return configurations;
        }

        private static bool MatchesWithReplacement(string configuration, string replacement, int numberOfHashes,
            [MaybeNullWhen(false)] out string matchedConfiguration)
        {
            var possibleConfiguration = Regex.Replace(configuration, replacement, 1);
            var count0 = possibleConfiguration.Count(c => c == '#');
            var count1 = possibleConfiguration.Count(c => c == '?');
            if (count0 <= numberOfHashes && numberOfHashes <= count0 + count1)
            {
                matchedConfiguration = possibleConfiguration;
                return true;
            }

            matchedConfiguration = null;
            return false;
        }
    }

    [GeneratedRegex("\\?")]
    private static partial Regex Unknowns();
}
