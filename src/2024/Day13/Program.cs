using System.Text.RegularExpressions;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var machines = new List<Machine>();

var i = 0;
while (i < lines.Length)
{
    var buttonA = Utils.ParseLine(lines[i++]);
    var buttonB = Utils.ParseLine(lines[i++]);
    var price = Utils.ParseLine(lines[i++]);
    i++;
    
    machines.Add(new Machine(buttonA, buttonB, price));
}

var part1 = machines.Sum(m => m.CalcCosts(0));
var part2 = machines.Sum(m => m.CalcCosts(10000000000000L));

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

internal static partial class Utils
{
    [GeneratedRegex(@"^[a-zA-Z ]+\: X[\+=]{1}(\d+), Y[\+=]{1}(\d+)$")]
    private static partial Regex LineMatcher();

    internal static Vec2 ParseLine(string line)
    {
        var match = LineMatcher().Match(line);
        return new Vec2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
    }
}

internal class Machine
{
    public Vec2 ButtonA { get; }
    public Vec2 ButtonB { get; }
    public Vec2 Price { get; }

    public Machine(Vec2 buttonA, Vec2 buttonB, Vec2 price)
    {
        ButtonA = buttonA;
        ButtonB = buttonB;
        Price = price;
    }

    public long CalcCosts(long offset)
    {
        var px = Price.X + offset;
        var py = Price.Y + offset;
        
        double d = ButtonA.X * ButtonB.Y - ButtonA.Y * ButtonB.X;
        double dx = px * ButtonB.Y - py * ButtonB.X;
        double dy = ButtonA.X * py - ButtonA.Y * px;
        
        var x = dx / d;
        var y = dy / d;
        
        if (x % 1 == 0 && y % 1 == 0)
        {
            return (long)x * 3 + (long)y;
        }

        return 0;
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
}
