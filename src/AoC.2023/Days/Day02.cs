using AoC.Shared;

namespace AoC._2023.Days;

public class Day02 : DayBase<List<Day02.Game>, int>
{
    protected override string Name => "Day 2: Cube Conundrum";

    protected override List<Game> ParseInput(IEnumerable<string> lines)
        => lines.Select(l =>
        {
            var parts = l.Split(": ");
            var id = int.Parse(parts[0][5..]);

            var inputParts = parts[1].Split("; ");

            var cubeSets = new List<CubeSet>();

            foreach (var inputPart in inputParts)
            {
                var cubeParts = inputPart.Split(", ");
                var cubes = new CubeSet();
                cubeSets.Add(cubes);

                foreach (var cubePart in cubeParts)
                {
                    var p = cubePart.Split(" ");
                    var amount = int.Parse(p[0]);
                    var color = p[1];
                    
                    cubes.Add(new Cube
                    {
                        Amount = amount,
                        Color = CubeColor.FromName(color),
                    });
                }
            }

            return new Game
            {
                Id = id,
                CubeSets = cubeSets,
            };
        }).ToList();

    protected override int RunPart1(List<Game> input)
        => input.Where(g =>
        {
            var reds = g.GetRequiredCubes(CubeColor.Red);
            var greens = g.GetRequiredCubes(CubeColor.Green);
            var blues = g.GetRequiredCubes(CubeColor.Blue);

            return reds <= 12 && greens <= 13 && blues <= 14;
        }).Sum(g => g.Id);

    protected override int RunPart2(List<Game> input)
        => input.Sum(g =>
        {
            var reds = g.GetRequiredCubes(CubeColor.Red);
            var greens = g.GetRequiredCubes(CubeColor.Green);
            var blues = g.GetRequiredCubes(CubeColor.Blue);

            return reds * greens * blues;
        });
    
    public class Game
    {
        public required int Id { get; init; }
        public required List<CubeSet> CubeSets { get; init; }

        public int GetRequiredCubes(CubeColor color)
        {
            return CubeSets.Max(c => c.GetRequiredCubes(color));
        }
    }

    public class CubeSet : List<Cube>
    {
        public int GetRequiredCubes(CubeColor color)
        {
            return this.FirstOrDefault(c => c.Color == color)?.Amount ?? 0;
        }
    }

    public class Cube
    {
        public required int Amount { get; init; }
        public required CubeColor Color { get; init; }
    }

    public class CubeColor
    {
        public static CubeColor Red { get; } = new("red");
        public static CubeColor Green { get; } = new("green");
        public static CubeColor Blue { get; } = new("blue");
        
        public string Name { get; }
        
        private CubeColor(string name)
        {
            Name = name;
        }

        public static CubeColor FromName(string name)
        {
            return name switch
            {
                "red" => Red,
                "green" => Green,
                "blue" => Blue,
                _ => throw new ArgumentOutOfRangeException(nameof(name), name, "unknown color name")
            };
        }
    }
}
