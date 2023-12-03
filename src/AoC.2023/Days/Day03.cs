using AoC.Shared;
using AoC.Shared.Math;

namespace AoC._2023.Days;

public sealed class Day03 : DayBase<Day03.Engine, int>
{
    protected override string Name => "Day 3: Gear Ratios";
    
    protected override Engine ParseInput(IEnumerable<string> lines)
    {
        var lineList = lines.ToList();
        var height = lineList.Count;
        var width = lineList[0].Length;

        var engine = new Engine();

        for (var y = 0; y < height; y++)
        {
            var line = lineList[y];
            for (var x = 0; x < width; x++)
            {
                var symbol = line[x];
                if (symbol == '.' || char.IsDigit(symbol))
                    continue;

                var positionsToLook = GetAdjacentPositions(x, y, width, height);
                for (var i = 0; i < positionsToLook.Count; i++)
                {
                    var position = positionsToLook[i];
                    var lineToLook = lineList[position.Y];
                    if (!char.IsDigit(lineToLook[position.X]))
                        continue;

                    var xStart = position.X;
                    while (xStart > 0 && char.IsDigit(lineToLook[xStart - 1]))
                        xStart--;

                    var xEnd = position.X;
                    while (xEnd < width - 1 && char.IsDigit(lineToLook[xEnd + 1]))
                    {
                        xEnd++;
                        positionsToLook.Remove(new Vec2<int>(xEnd, position.Y));
                    }

                    var number = int.Parse(lineToLook.AsSpan().Slice(xStart, xEnd - xStart + 1));
                    engine.Add(new EnginePart
                    {
                        Symbol = symbol,
                        SymbolPosition = new Vec2<int>(x, y),
                        Number = number,
                        NumberPositionStart = new Vec2<int>(xStart, position.Y),
                        NumberPositionEnd = new Vec2<int>(xEnd, position.Y),
                    });
                }
            }
        }

        return engine;
    }

    protected override int RunPart1(Engine input)
        => input.Sum(p => p.Number);

    protected override int RunPart2(Engine input)
        => input
            .Where(p => p.Symbol == '*')
            .GroupBy(p => p.SymbolPosition)
            .Where(g => g.Count() == 2)
            .Sum(g => g.First().Number * g.Last().Number);

    private static List<Vec2<int>> GetAdjacentPositions(int x, int y, int width, int height)
    {
        var list = new List<Vec2<int>>();

        // above
        if (x > 0 && y > 0) list.Add(new Vec2<int>(x - 1, y - 1));
        if (y > 0) list.Add(new Vec2<int>(x, y - 1));
        if (x < width - 1 && y > 0) list.Add(new Vec2<int>(x + 1, y - 1));
        
        // same line
        if (x > 0) list.Add(new Vec2<int>(x - 1, y));
        if (x < width - 1) list.Add(new Vec2<int>(x + 1, y));
        
        // below
        if (y > 0 && x > 0) list.Add(new Vec2<int>(x - 1, y + 1));
        if (x > 0) list.Add(new Vec2<int>(x, y + 1));
        if (y < height - 1 && x > 0) list.Add(new Vec2<int>(x + 1, y + 1));
        
        return list;
    }
    
    public class Engine : List<EnginePart> { }

    public struct EnginePart
    {
        public required char Symbol { get; init; }
        public required Vec2<int> SymbolPosition { get; init; }
        
        public required int Number { get; init; }
        public required Vec2<int> NumberPositionStart { get; init; }
        public required Vec2<int> NumberPositionEnd { get; init; }
    }
}
