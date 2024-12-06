#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var directions = new Dictionary<char, Vec2>
{
    ['^'] = new(0, -1),
    ['v'] = new(0, 1),
    ['<'] = new(-1, 0),
    ['>'] = new(1, 0),
};

var rotations = new Dictionary<Vec2, Vec2>
{
    [new Vec2(0, -1)] = new(1, 0),
    [new Vec2(1, 0)] = new(0, 1),
    [new Vec2(0, 1)] = new(-1, 0),
    [new Vec2(-1, 0)] = new(0, -1),
};

var maxY = lines.Length;
var maxX = lines[0].Length;
var map = new bool[maxY, maxX];

Vec2 pos = default;
Vec2 dir = default;

for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        var chr = line[x];
        map[y, x] = chr == '#';

        if (chr is '^' or 'v' or '<' or '>')
        {
            pos = new Vec2(x, y);
            dir = directions[chr];
        }
    }
}

var startPos = pos;
var startDir = dir;

var guardLines = new List<Line2>();
var lineStart = pos;

while (IsOnMap(pos, maxX, maxY))
{
    var newPos = pos + dir;
    if (!IsOnMap(newPos, maxX, maxY))
    {
        guardLines.Add(new Line2(lineStart, pos, dir));
        break;
    }
    
    if (map[newPos.Y, newPos.X])
    {
        guardLines.Add(new Line2(lineStart, pos, dir));
        lineStart = pos;
        dir = rotations[dir];
    }
    else
    {
        pos = newPos;
    }
}

var part1 = guardLines.SelectMany(l => l.GetAllPoints()).ToHashSet().Count;

Console.WriteLine($"Part 1: {part1}");

var part2 = 0;

for (var y = 0; y < maxY; y++)
{
    for (var x = 0; x < maxX; x++)
    {
        if (map[y, x] || startPos.Equals(new Vec2(x, y)))
            continue;
        
        var mapCopy = new bool[maxY, maxX];
        Array.Copy(map, mapCopy, mapCopy.Length);
        
        mapCopy[y, x] = true;

        if (CheckForLoop(mapCopy, startPos, startDir, maxX, maxY, rotations))
        {
            part2++;
        }
    }
}

Console.WriteLine($"Part 2: {part2}");

/* Part 2 with Ray Tracing (not working)
HashSet<Vec2> part2 = [];

for (var i = 3; i < guardLines.Count; i++)
{
    var line2 = guardLines[i];
    
    for (var j = 0; j < i - 2; j++)
    {
        var line1 = guardLines[j];
        
        if (line1.TryGetIntersectionWithInfiniteLine(line2, out var intersection) && rotations[line2.Direction].Equals(line1.Direction))
        {
            part2.Add(intersection);
        }
    }
}

Console.WriteLine($"Part 2: {part2.Count}");
*/
return;

static bool IsOnMap(Vec2 pos, int maxX, int maxY)
{
    return pos.X < maxX && pos.Y < maxY && pos.X >= 0 && pos.Y >= 0;
}

static bool CheckForLoop(bool[,] map, Vec2 pos, Vec2 dir, int maxX, int maxY, Dictionary<Vec2, Vec2> rotations)
{
    HashSet<State> visited = [new(pos, dir)];

    while (IsOnMap(pos, maxX, maxY))
    {
        var newPos = pos + dir;
        if (!IsOnMap(newPos, maxX, maxY))
            return false;
    
        if (map[newPos.Y, newPos.X])
        {
            dir = rotations[dir];
        }
        else
        {
            pos = newPos;
        }

        if (!visited.Add(new State(pos, dir)))
            return true;
    }

    return false;
}

internal readonly struct Vec2 : IEquatable<Vec2>
{
    public int X { get; }
    public int Y { get; }

    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X + b.X, a.Y + b.Y);
    }

    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X - b.X, a.Y - b.Y);
    }

    public static int operator *(Vec2 a, Vec2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }
    
    public bool Equals(Vec2 other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

internal readonly struct State : IEquatable<State>
{
    public Vec2 Position { get; }
    public Vec2 Direction { get; }

    public State(Vec2 position, Vec2 direction)
    {
        Position = position;
        Direction = direction;
    }

    public bool Equals(State other)
    {
        return Position.Equals(other.Position) && Direction.Equals(other.Direction);
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Direction);
    }
}

internal readonly struct Line2 : IEquatable<Line2>
{
    public Vec2 Start { get; }
    public Vec2 End { get; }
    public Vec2 Direction { get; }

    public Line2(Vec2 start, Vec2 end, Vec2 direction)
    {
        Start = start;
        End = end;
        Direction = direction;
    }

    public IEnumerable<Vec2> GetAllPoints()
    {
        yield return Start;

        var pos = Start;
        while (!pos.Equals(End))
        {
            pos += Direction;
            yield return pos;
        }
    }
    
    public bool TryGetIntersectionWithInfiniteLine(Line2 segment, out Vec2 intersection)
    {
        intersection = default;

        var lineDir = End - Start;
        var segmentDir = segment.End - segment.Start;
        var denominator = lineDir * segmentDir;

        if (denominator == 0)
            return false;

        var startDiff = segment.Start - Start;
        var t = (float) (startDiff * lineDir) / denominator;

        if (t is < 0 or > 1)
            return false;

        intersection = new Vec2((int)(segment.Start.X + t * segmentDir.X), (int)(segment.Start.Y + t * segmentDir.Y));
        return true;
    }

    public bool Equals(Line2 other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End) && Direction.Equals(other.Direction);
    }

    public override bool Equals(object? obj)
    {
        return obj is Line2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End, Direction);
    }

    public override string ToString()
    {
        return $"{Start} -> {End} <{Direction}>";
    }
}
