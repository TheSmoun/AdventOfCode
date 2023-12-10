using System.Diagnostics;
using AoC.Shared;
using AoC.Shared.Math;
using MoreLinq;

namespace AoC._2023.Days;

public class Day10 : DayBase<Day10.Map, int>
{
    protected override string Name => "Day 10: Pipe Maze";
    
    protected override Map ParseInput(IEnumerable<string> lines)
    {
        var lineList = lines.ToList();
        var tokens = new char[lineList[0].Length, lineList.Count];
        var pipes = new Dictionary<Vec2<int>, Pipe>();
        var startPosition = new Vec2<int>(-1, -1);

        for (var y = 0; y < lineList.Count; y++)
        {
            var line = lineList[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                tokens[x, y] = c;
                
                if (c == 'S')
                {
                    startPosition = new Vec2<int>(x, y);
                }
                else if (c != '.')
                {
                    var position = new Vec2<int>(x, y);
                    pipes.Add(position, Pipe.FromToken(c, position));
                }
            }
        }

        pipes.Add(startPosition, Pipe.FromAdjacentPipes(startPosition, pipes));
        return new Map(pipes, startPosition, tokens);
    }

    protected override int RunPart1(Map input)
    {
        var position2Count = new Dictionary<Vec2<int>, int>
        {
            [input.StartPosition] = 0
        };

        var queue = new Queue<Vec2<int>>();
        var startPipe = input.Pipes[input.StartPosition];
        queue.Enqueue(startPipe.Position + startPipe.NextOffset);
        queue.Enqueue(startPipe.Position + startPipe.PrefOffset);

        while (queue.Count > 0)
        {
            var position = queue.Dequeue();
            if (position2Count.ContainsKey(position))
                continue;
            
            var pipe = input.Pipes[position];
            
            if (position2Count.TryGetValue(position + pipe.PrefOffset, out var value))
            {
                position2Count[position] = value + 1;
                queue.Enqueue(pipe.Position + pipe.NextOffset);
            }
            else if (position2Count.TryGetValue(position + pipe.NextOffset, out var value1))
            {
                position2Count[position] = value1 + 1;
                queue.Enqueue(pipe.Position + pipe.PrefOffset);
            }
        }

        return position2Count.Values.Max();
    }

    protected override int RunPart2(Map input)
    {
        return 0;
    }

    public class Map
    {
        public Dictionary<Vec2<int>, Pipe> Pipes { get; }
        public Vec2<int> StartPosition { get; }
        public char[,] Tokens { get; }

        public Map(Dictionary<Vec2<int>, Pipe> pipes, Vec2<int> startPosition, char[,] tokens)
        {
            Pipes = pipes;
            StartPosition = startPosition;
            Tokens = tokens;
        }
    }

    public class Pipe
    {
        public Vec2<int> Position { get; }
        public Vec2<int> NextOffset { get; }
        public Vec2<int> PrefOffset { get; }

        private Pipe(Vec2<int> position, Vec2<int> nextOffset, Vec2<int> prefOffset)
        {
            Position = position;
            NextOffset = nextOffset;
            PrefOffset = prefOffset;
        }
        
        public static Pipe FromToken(char token, Vec2<int> position)
        {
            return token switch
            {
                '|' => new Pipe(position, Offsets.South, Offsets.North),
                '-' => new Pipe(position, Offsets.West, Offsets.East),
                'L' => new Pipe(position, Offsets.East, Offsets.North),
                'J' => new Pipe(position, Offsets.West, Offsets.North),
                '7' => new Pipe(position, Offsets.West, Offsets.South),
                'F' => new Pipe(position, Offsets.East, Offsets.South),
                _ => throw new UnreachableException()
            };
        }

        public static Pipe FromAdjacentPipes(Vec2<int> position, Dictionary<Vec2<int>, Pipe> pipes)
        {
            for (var i = 0; i < Offsets.AllOffsets.Count; i++)
            {
                for (var j = 0; j < Offsets.AllOffsets.Count; j++)
                {
                    if (i == j)
                        continue;

                    var first = Offsets.AllOffsets[i];
                    var second = Offsets.AllOffsets[j];
                    
                    if (pipes.TryGetValue(position + first, out var p1) && p1.ConnectsTo(position)
                        && pipes.TryGetValue(position + second, out var p2) && p2.ConnectsTo(position))
                    {
                        return new Pipe(position, first, second);
                    }
                }
            }

            throw new UnreachableException();
        }

        public bool ConnectsTo(Vec2<int> position)
        {
            return Position + NextOffset == position || Position + PrefOffset == position;
        }
    }

    private static class Offsets
    {
        public static Vec2<int> North { get; } = new(0, -1);
        public static Vec2<int> South { get; } = new(0, 1);
        public static Vec2<int> East { get; } = new(1, 0);
        public static Vec2<int> West { get; } = new(-1, 0);
        public static List<Vec2<int>> AllOffsets { get; } = new()
        {
            North,
            South,
            East,
            West,
        };
    }
}
