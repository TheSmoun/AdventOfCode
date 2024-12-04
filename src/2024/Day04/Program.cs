#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

List<Vec2> offsets = [
    new(-1, -1), new(0, -1), new(1, -1),
    new(-1, 0), new(0, 0), new(1, 0),
    new(-1, 1), new(0, 1), new(1, 1),
];

var grid = new Dictionary<Vec2, char>();
var xPositions = new List<Vec2>();
var aPositions = new List<Vec2>();

for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        var c = line[x];
        var pos = new Vec2(x, y);
        grid[pos] = c;
        
        if (c == 'X')
        {
            xPositions.Add(pos);
        }

        if (c == 'A')
        {
            aPositions.Add(pos);
        }
    }
}

var part1 = 0;

foreach (var xPosition in xPositions)
{
    foreach (var offset in offsets)
    {
        if (IsXmas(xPosition, offset, grid))
        {
            part1++;
        }
    }
}

Console.WriteLine($"Part 1: {part1}");

var part2 = 0;

foreach (var aPosition in aPositions)
{
    if (IsMasX(aPosition, grid))
    {
        part2++;
    }
}

Console.WriteLine($"Part 2: {part2}");

return;

static bool IsXmas(Vec2 xPosition, Vec2 offset, Dictionary<Vec2, char> grid)
{
    var mPosition = xPosition + offset;
    if (!grid.TryGetValue(mPosition, out var m) || m != 'M')
        return false;

    var aPosition = mPosition + offset;
    if (!grid.TryGetValue(aPosition, out var a) || a != 'A')
        return false;

    var sPosition = aPosition + offset;
    if (!grid.TryGetValue(sPosition, out var s) || s != 'S')
        return false;

    return true;
}

static bool IsMasX(Vec2 aPosition, Dictionary<Vec2, char> grid)
{
    if (!grid.TryGetValue(aPosition + new Vec2(-1, -1), out var tl)
        || !grid.TryGetValue(aPosition + new Vec2(-1, 1), out var tr)
        || !grid.TryGetValue(aPosition + new Vec2(1, -1), out var bl)
        || !grid.TryGetValue(aPosition + new Vec2(1, 1), out var br))
        return false;

    if (tl != 'M' && tl != 'S' || tr != 'M' && tr != 'S' || bl != 'M' && bl != 'S' || br != 'M' && br != 'S')
        return false;

    if (tl == tr && bl == br && tl != bl)
        return true;

    if (tl == bl && tr == br && tl != tr)
        return true;
    
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
