using AoC.Shared;
using AoC.Shared.Extensions;
using AoC.Shared.Math;

namespace AoC._2023.Days;

public class Day11 : DayBase<Day11.Universe, long>
{
    protected override string Name => "Day 11: Cosmic Expansion";
    
    protected override Universe ParseInput(IEnumerable<string> lines)
    {
        var galaxies = new List<Galaxy>();
        var lineList = lines.ToList();

        for (var y = 0; y < lineList.Count; y++)
        {
            var line = lineList[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    galaxies.Add(new Galaxy
                    {
                        Number = galaxies.Count + 1,
                        Position = new Vec2<long>(x, y),
                    });
                }
            }
        }

        return new Universe
        {
            Galaxies = galaxies,
            Width = lineList[0].Length,
            Height = lineList.Count,
        };
    }

    protected override long RunPart1(Universe input) => RunInternal(input, 1L);

    protected override long RunPart2(Universe input) => RunInternal(input, 999999L);

    private static long RunInternal(Universe input, long expansionRate)
    {
        input.Expand(expansionRate);
        
        return input.Galaxies
            .Pairs()
            .Select(t => t.Item2.Position - t.Item1.Position)
            .Sum(d => Math.Abs(d.X) + Math.Abs(d.Y));
    }

    public class Universe
    {
        public required List<Galaxy> Galaxies { get; init; }
        public required long Width { get; set; }
        public required long Height { get; set; }

        public void Expand(long expansionRate)
        {
            ExpandVertical(expansionRate);
            ExpandHorizontal(expansionRate);
        }

        private void ExpandVertical(long expansionRate)
        {
            for (var y = 0L; y < Height; y++)
            {
                if (Galaxies.All(g => g.Position.Y != y) && Galaxies.Any(g => g.Position.Y > y))
                {
                    for (var i = 0; i < Galaxies.Count; i++)
                    {
                        if (Galaxies[i].Position.Y > y)
                        {
                            Galaxies[i].Position += new Vec2<long>(0, expansionRate);
                        }
                    }

                    Height += expansionRate;
                    y += expansionRate;
                }
            }
        }

        private void ExpandHorizontal(long expansionRate)
        {
            for (var x = 0L; x < Width; x++)
            {
                if (Galaxies.All(g => g.Position.X != x) && Galaxies.Any(g => g.Position.X > x))
                {
                    for (var i = 0; i < Galaxies.Count; i++)
                    {
                        if (Galaxies[i].Position.X > x)
                        {
                            Galaxies[i].Position += new Vec2<long>(expansionRate, 0);
                        }
                    }

                    Width += expansionRate;
                    x += expansionRate;
                }
            }
        }
    }

    public class Galaxy
    {
        public required int Number { get; init; }
        public required Vec2<long> Position { get; set; }
    }
}
