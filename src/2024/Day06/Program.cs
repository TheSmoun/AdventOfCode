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

return;

static bool IsOnMap(Vec2 pos, int maxX, int maxY)
{
    return pos.X < maxX && pos.Y < maxY && pos.X >= 0 && pos.Y >= 0;
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
    
    public bool TryGetIntersection(Line2 line, out Vec2 intersection)
    {
        intersection = default;

        var x1 = Start.X;
        var y1 = Start.Y;
        var x2 = End.X;
        var y2 = End.Y;
        var x3 = line.Start.X;
        var y3 = line.Start.Y;
        var x4 = line.End.X;
        var y4 = line.End.Y;

        var denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (denominator == 0)
            return false;

        var numerator1 = x1 * y2 - y1 * x2;
        var numerator2 = x3 * y4 - y3 * x4;

        var px = (numerator1 * (x3 - x4) - (x1 - x2) * numerator2) / denominator;
        var py = (numerator1 * (y3 - y4) - (y1 - y2) * numerator2) / denominator;

        var potentialIntersection = new Vec2(px, py);

        if (IsPointOnLineSegment(potentialIntersection) && line.IsPointOnLineSegment(potentialIntersection))
        {
            intersection = potentialIntersection;
            return true;
        }

        return false;
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
    
    private bool IsPointOnLineSegment(Vec2 point)
    {
        var x = point.X;
        var y = point.Y;
        var x1 = Start.X;
        var y1 = Start.Y;
        var x2 = End.X;
        var y2 = End.Y;

        return x >= Math.Min(x1, x2) && x <= Math.Max(x1, x2) &&
                y >= Math.Min(y1, y2) && y <= Math.Max(y1, y2) &&
               (x2 - x1) * (y - y1) == (y2 - y1) * (x - x1);
    }
}
