#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var height = lines.Length;
var width = lines[0].Length;
var heights = new int[height, width];

for (var y = 0; y < height; y++)
{
    var line = lines[y];
    for (var x = 0; x < width; x++)
    {
        heights[y, x] = line[x] - '0';
    }
}

var reachableTops = new List<List<Vec2>>();
for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        if (heights[y, x] == 0)
        {
            reachableTops.Add(FindTops([new Vec2(x, y)], [], new Vec2(x, y)));
        }
    }
}

Console.WriteLine($"Part 1: {reachableTops.Sum(t => t.Distinct().Count())}");
Console.WriteLine($"Part 2: {reachableTops.Sum(t => t.Count)}");

return;

List<Vec2> FindTops(List<Vec2> path, List<Vec2> tops, Vec2 pos)
{
    if (heights[pos.Y, pos.X] == 9)
    {
        tops.Add(pos);
    }
    else
    {
        for (var y = -1; y <= 1; y++)
        {
            for (var x = -1; x <= 1; x++)
            {
                if (y != 0 && x != 0)
                    continue;
                
                var newPos = pos + new Vec2(x, y);
                if (newPos.Y < 0 || newPos.Y >= height || newPos.X < 0 || newPos.X >= width)
                    continue;
                
                if (heights[newPos.Y, newPos.X] - heights[pos.Y, pos.X] == 1 && !path.Contains(newPos))
                    FindTops([..path], tops, newPos);
            }
        }
    }

    return tops;
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
