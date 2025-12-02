using System.Diagnostics;
using System.Text.RegularExpressions;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var ids = lines[0].Split(',').Select(r =>
{
    var parts = r.Split('-');
    return new IdRange
    {
        Min = long.Parse(parts[0]),
        Max = long.Parse(parts[1]),
    };
}).SelectMany(r => r.GetIds()).ToList();

var sum1 = 0L;
var sum2 = 0L;

var regex1 = Utils.Part1Pattern();
var regex2 = Utils.Part2Pattern();

foreach (var id in ids)
{
    var s = id.ToString();
    if (regex1.IsMatch(s))
    {
        sum1 += id;
        sum2 += id;
    }
    else if (regex2.IsMatch(s))
    {
        sum2 += id;
    }
}

Console.WriteLine("Part 1: " + sum1);
Console.WriteLine("Part 2: " + sum2);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

class IdRange
{
    public long Min { get; set; }
    public long Max { get; set; }

    public IEnumerable<long> GetIds()
    {
        for (var i = Min; i <= Max; i++)
        {
            yield return i;
        }
    }
}

static partial class Utils
{
    [GeneratedRegex(@"^(\d+)\1$")]
    internal static partial Regex Part1Pattern();
    
    [GeneratedRegex(@"^(\d+)\1+$")]
    internal static partial Regex Part2Pattern();
}
