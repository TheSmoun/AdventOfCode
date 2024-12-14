using System.Text;
using System.Text.RegularExpressions;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

const int width = 101;
const int height = 103;

var dimensions = new Vec2(width, height);
var halfDimensions = new Vec2(width / 2, height / 2);

var robots = lines.Select(Utils.ParseLine).ToArray();
var quadrants = new int[4];

foreach (var robot in robots)
{
    var position = robot.Move(100) % dimensions;
    var quadrant = Utils.GetQuadrant(position, halfDimensions);
    if (quadrant >= 0)
    {
        quadrants[quadrant]++;
    }
}

var part1 = quadrants.Aggregate(1, (x, y) => x * y);
Console.WriteLine($"Part 1: {part1}");

var part2 = 0;
while (true)
{
    var positions = robots.Select(r => r.Move(part2) % dimensions).ToArray();
    if (Utils.Print(positions, width, height).Contains("#################"))
        break;

    part2++;
}

Console.Write($"Part 2: {part2}");

internal static partial class Utils
{
    [GeneratedRegex(@"^p=(\-?\d+),(\-?\d+) v=(\-?\d+),(\-?\d+)$")]
    private static partial Regex LineMatcher();

    internal static Robot ParseLine(string line)
    {
        var match = LineMatcher().Match(line);
        var position = new Vec2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        var velocity = new Vec2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
        return new Robot(position, velocity);
    }

    internal static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    internal static int GetQuadrant(Vec2 position, Vec2 halfDimensions)
    {
        position -= halfDimensions;
        if (position.X == 0 || position.Y == 0)
            return -1;
        
        return 2 * Convert.ToInt32(position.X > 0) + Convert.ToInt32(position.Y > 0);
    }

    internal static string Print(Vec2[] positions, int width, int height)
    {
        var plot = new char[height, width];
        foreach (var position in positions)
        {
            plot[position.Y, position.X] = '#';
        }
        
        var sb = new StringBuilder();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                sb.Append(plot[y, x] == '#' ? '#' : ' ');
            }
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}

internal record Robot(Vec2 Position, Vec2 Velocity)
{
    public Vec2 Move(int steps)
    {
        return Position + Velocity * steps;
    }
}

internal readonly struct Vec2
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

    public static Vec2 operator *(Vec2 a, int f)
    {
        return new Vec2(a.X * f, a.Y * f);
    }

    public static Vec2 operator %(Vec2 a, Vec2 b)
    {
        return new Vec2(Utils.Mod(a.X, b.X), Utils.Mod(a.Y, b.Y));
    }
}
