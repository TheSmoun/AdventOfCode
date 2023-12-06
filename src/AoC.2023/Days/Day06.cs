using AoC.Shared;

namespace AoC._2023.Days;

public class Day06 : DayBase<(string Times, string Distances), long>
{
    protected override string Name => "Day 6: Wait For It";
    
    protected override (string Times, string Distances) ParseInput(IEnumerable<string> lines)
    {
        var lineList = lines.ToList();
        return (lineList[0][11..], lineList[1][11..]);
    }

    protected override long RunPart1((string Times, string Distances) input)
    {
        return input.Times.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Zip(input.Distances.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(tuple => new Race
            {
                Time = long.Parse(tuple.First),
                Distance = long.Parse(tuple.Second),
            })
            .Aggregate(1L, (i, race) => i * race.Simulate());
    }

    protected override long RunPart2((string Times, string Distances) input)
    {
        var race = new Race
        {
            Time = long.Parse(input.Times.Replace(" ", string.Empty)),
            Distance = long.Parse(input.Distances.Replace(" ", string.Empty)),
        };

        return race.Simulate();
    }

    public class Race
    {
        public required long Time { get; init; }
        public required long Distance { get; init; }

        public long Simulate()
        {
            var count = 0L;
            for (var i = 1L; i <= Time; i++)
            {
                if (IsEnough(i))
                    count++;
            }
            
            return count;
        }

        private bool IsEnough(long buttonHoldTime)
        {
            return buttonHoldTime * (Time - buttonHoldTime) > Distance;
        }
    }
}
