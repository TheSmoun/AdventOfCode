#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var i = 0;
string line;

var rules = new HashSet<OrderRule>();
while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var span = line.AsSpan();
    var left = int.Parse(span.Slice(0, 2));
    var right = int.Parse(span.Slice(3, 2));
    rules.Add(new OrderRule(left, right));
}

var prints = new List<List<int>>();
while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var print = line.Split(',').Select(int.Parse).ToList();
    prints.Add(print);
}

var comparer = new PrintOrderRuleComparer(rules);

var part1 = 0;
var part2 = 0;

foreach (var print in prints)
{
    var orderedPrint = print.Order(comparer).ToList();
    if (print.SequenceEqual(orderedPrint))
    {
        part1 += print[print.Count / 2];
    }
    else
    {
        part2 += orderedPrint[orderedPrint.Count / 2];
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

internal readonly struct OrderRule : IEquatable<OrderRule>
{
    public int Left { get; }
    public int Right { get; }

    public OrderRule(int left, int right)
    {
        Left = left;
        Right = right;
    }

    public bool Equals(OrderRule other)
    {
        return Left == other.Left && Right == other.Right;
    }

    public override bool Equals(object? obj)
    {
        return obj is OrderRule other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Left, Right);
    }
}

internal class PrintOrderRuleComparer : IComparer<int>
{
    private readonly HashSet<OrderRule> _rules;
    
    public PrintOrderRuleComparer(HashSet<OrderRule> rules)
    {
        _rules = rules;
    }
    
    public int Compare(int x, int y)
    {
        if (_rules.Contains(new OrderRule(x, y)))
            return -1;
        
        if (_rules.Contains(new OrderRule(y, x)))
            return 1;
        
        return 0;
    }
}
