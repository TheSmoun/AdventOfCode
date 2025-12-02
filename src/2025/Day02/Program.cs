using System.Text.RegularExpressions;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var idRanges = lines[0].Split(',').Select(r =>
{
    var parts = r.Split('-');
    return new IdRange
    {
        Min = long.Parse(parts[0]),
        Max = long.Parse(parts[1]),
    };
}).ToList();

var ids = idRanges.SelectMany(r => r.GetIds()).ToList();

var sum1 = 0L;
var sum2 = 0L;

var regex1 = Utils.Part1Pattern();
var regex2 = Utils.Part2Pattern();

foreach (var id in ids)
{
    if (regex1.IsMatch(id))
    {
        sum1 += long.Parse(id);
    }

    if (regex2.IsMatch(id))
    {
        sum2 += long.Parse(id);
    }
}

Console.WriteLine("Part 1: " + sum1);
Console.WriteLine("Part 2: " + sum2);

return;

class IdRange
{
    public long Min { get; set; }
    public long Max { get; set; }

    public IEnumerable<string> GetIds()
    {
        for (var i = Min; i <= Max; i++)
        {
            yield return $"{i}";
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
