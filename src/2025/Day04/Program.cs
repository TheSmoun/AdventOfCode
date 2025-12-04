using System.Diagnostics;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

List<Vec2> offsets = [
    new(-1, -1), new(0, -1), new(1, -1),
    new(-1, 0), new(1, 0),
    new(-1, 1), new(0, 1), new(1, 1),
];

var paperRolls = new HashSet<Vec2>();

for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        if (line[x] == '@')
        {
            paperRolls.Add(new Vec2(x, y));
        }
    }
}

var count = 0;

foreach (var paperRoll in paperRolls)
{
    var neighbors = 0;
    foreach (var offset in offsets)
    {
        if (paperRolls.Contains(paperRoll + offset))
        {
            neighbors++;
        }
    }

    if (neighbors < 4)
    {
        count++;
    }
}

Console.WriteLine("Part 1: " + count);

var startCount = paperRolls.Count;
var removed = true;

while (removed)
{
    removed = false;
    
    var paperRollsToRemove = new List<Vec2>();

    foreach (var paperRoll in paperRolls)
    {
        var neighbors = 0;
        foreach (var offset in offsets)
        {
            if (paperRolls.Contains(paperRoll + offset))
            {
                neighbors++;
            }
        }

        if (neighbors < 4)
        {
            paperRollsToRemove.Add(paperRoll);
            removed = true;
        }
    }
    
    foreach (var paperRoll in paperRollsToRemove)
    {
        paperRolls.Remove(paperRoll);
    }
}

var result = startCount - paperRolls.Count;

Console.WriteLine("Part 2: " + result);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

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
