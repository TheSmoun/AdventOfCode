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

var positions = new HashSet<Vec2> { startPos };

while (IsOnMap(pos, maxX, maxY))
{
    var newPos = pos + dir;
    if (!IsOnMap(newPos, maxX, maxY))
        break;
    
    if (map[newPos.Y, newPos.X])
    {
        dir = rotations[dir];
    }
    else
    {
        pos = newPos;
        positions.Add(pos);
    }
}

Console.WriteLine($"Part 1: {positions.Count}");

var part2 = 0;

foreach (var position in positions)
{
    if (startPos.Equals(position))
        continue;
    
    var mapCopy = new bool[maxY, maxX];
    Array.Copy(map, mapCopy, mapCopy.Length);
    
    mapCopy[position.Y, position.X] = true;

    if (CheckForLoop(mapCopy, startPos, startDir, maxX, maxY, rotations))
    {
        part2++;
    }
}

Console.WriteLine($"Part 2: {part2}");

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
