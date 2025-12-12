using System.Diagnostics;
using MoreLinq;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var parts = lines.Split(string.IsNullOrWhiteSpace).Select(c => c.ToList()).ToList();
var count = parts[^1].Count(l =>
{
    var lr = l.Split(':');
    var wh = lr[0].Split('x').Select(int.Parse).ToArray();
    var indexes = lr[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    return indexes.Sum() * 7 < wh[0] * wh[1];
});

Console.WriteLine("Part 1: " + count);
Console.WriteLine("Part 2: " + string.Empty);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif
