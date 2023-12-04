using AoC.Shared;

namespace AoC._2023.Days;

public sealed class Day04 : DayBase<List<Day04.Scratchcard>, int>
{
    protected override string Name => "Day 4: Scratchcards";

    protected override List<Scratchcard> ParseInput(IEnumerable<string> lines)
        => lines.Select(l =>
        {
            var parts = l.Split(new[] { ": ", " | " }, StringSplitOptions.RemoveEmptyEntries);
            var number = int.Parse(parts[0][5..]);
            var winningNumbers = ParseNumbers(parts[1]);
            var elvNumbers = ParseNumbers(parts[2]);

            var matches = elvNumbers.Intersect(winningNumbers).ToList();
            var winningScore = matches.Count == 0 ? 0 : matches.Skip(1).Aggregate(1, (i, _) => i * 2);
            
            return new Scratchcard
            {
                Number = number,
                WinningNumberCount = matches.Count,
                WinningScore = winningScore,
            };
        }).ToList();

    protected override int RunPart1(List<Scratchcard> input)
        => input.Sum(c => c.WinningScore);

    protected override int RunPart2(List<Scratchcard> input)
    {
        foreach (var card in input.Where(c => c.WinningNumberCount > 0))
        {
            for (var i = 0; i < card.WinningNumberCount; i++)
            {
                input[card.Number + i].Copies += card.Copies;
            }
        }

        return input.Sum(c => c.Copies);
    }

    private static IEnumerable<int> ParseNumbers(string numberPart)
        => numberPart
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);

    public class Scratchcard
    {
        public required int Number { get; init; }
        public required int WinningNumberCount { get; init; }
        public required int WinningScore { get; init; }
        public int Copies { get; set; } = 1;
    }
}
