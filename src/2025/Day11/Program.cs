using System.Diagnostics;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var paths = new Dictionary<string, string[]>();
foreach (var line in lines)
{
    var parts = line.Split(' ');
    paths[parts[0][..^1]] = parts[1..];
}

Console.WriteLine("Part 1: " + CountPaths("you"));

// TODO

Console.WriteLine("Part 2: " + string.Empty);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

int CountPaths(string node)
{
    return paths[node].Contains("out") ? 1 : paths[node].Sum(CountPaths);
}
