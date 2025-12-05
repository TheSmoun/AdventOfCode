using System.Diagnostics;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var i = 0L;
string line;

var ranges = new List<IdRange>();
while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var parts = line.Split('-');
    var min = long.Parse(parts[0]);
    var max = long.Parse(parts[1]);
    var range = new IdRange(min, max);
    ranges.Add(range);
}

var ids = new List<long>();
while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var id = long.Parse(line);
    ids.Add(id);
}

var count = 0;
foreach (var id in ids)
{
    foreach (var range in ranges)
    {
        if (range.Contains(id))
        {
            count++;
            break;
        }
    }
}

Console.WriteLine("Part 1: " + count);

i = 0L;
ranges.Sort();

var sum = 0L;

foreach (var r in ranges)
{
    var range = r;
    if (i >= range.Min)
    {
        range = new IdRange(i + 1, range.Max);
    }

    if (range.Min <= range.Max)
    {
        sum += range.Count;
    }

    i = Math.Max(i, range.Max);
}

Console.WriteLine("Part 2: " + sum);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

internal readonly struct IdRange : IComparable<IdRange>
{
    public long Min { get; }
    public long Max { get; }
    public long Count => Max - Min + 1;
    
    public IdRange(long min, long max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(long value)
    {
        return value >= Min && value <= Max;
    }

    public int CompareTo(IdRange other)
    {
        var minComparison = Min.CompareTo(other.Min);
        if (minComparison != 0) return minComparison;
        return Max.CompareTo(other.Max);
    }
}
