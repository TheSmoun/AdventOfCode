#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var sensors = new List<Sensor>();
var mapTopLeft = new Vec2(0, 0);
var mapBottomRight = new Vec2(lines[0].Length - 1, lines.Length - 1);

for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        var symbol = line[x];
        if (symbol != '.')
        {
            sensors.Add(new Sensor(x, y, symbol));
        }
    }
}

var part1 = new HashSet<Vec2>();
var part2 = new HashSet<Vec2>();

for (var i = 0; i < sensors.Count - 1; i++)
{
    var sensorA = sensors[i];

    for (var j = i + 1; j < sensors.Count; j++)
    {
        var sensorB = sensors[j];
        if (sensorA.Symbol != sensorB.Symbol)
            continue;
        
        var diff = sensorB.Position - sensorA.Position;

        var antinodeA = sensorA.Position - diff;
        if (antinodeA.IsWithinBounds(mapTopLeft, mapBottomRight))
            part1.Add(antinodeA);
        
        var antinodeB = sensorB.Position + diff;
        if (antinodeB.IsWithinBounds(mapTopLeft, mapBottomRight))
            part1.Add(antinodeB);
        
        var gcd = Gcd(Math.Abs(diff.X), Math.Abs(diff.Y));
        
        var step = new Vec2(diff.X / gcd, diff.Y / gcd);

        var antinode = sensorA.Position;
        while (antinode.IsWithinBounds(mapTopLeft, mapBottomRight))
        {
            part2.Add(antinode);
            antinode -= step;
        }
        
        antinode = sensorB.Position;
        while (antinode.IsWithinBounds(mapTopLeft, mapBottomRight))
        {
            part2.Add(antinode);
            antinode += step;
        }
    }
}

Console.WriteLine($"Part 1: {part1.Count}");
Console.WriteLine($"Part 2: {part2.Count}");

return;

static int Gcd(int a, int b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }

    return a;
}

internal readonly struct Sensor
{
    public Vec2 Position { get; }
    public char Symbol { get; }

    public Sensor(int x, int y, char symbol)
    {
        Position = new Vec2(x, y);
        Symbol = symbol;
    }
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

    public bool IsWithinBounds(Vec2 topLeft, Vec2 bottomRight)
    {
        return X >= topLeft.X && Y >= topLeft.Y && X <= bottomRight.X && Y <= bottomRight.Y;
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
