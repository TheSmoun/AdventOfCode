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

var counts = new Dictionary<(string, bool, bool), long>();

Console.WriteLine("Part 2: " + CountPaths2("svr", false, false));

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

int CountPaths(string node)
{
    return paths[node].Contains("out") ? 1 : paths[node].Sum(CountPaths);
}

long CountPaths2(string node, bool dac, bool fft)
{
    var key = (node, dac, fft);
    if (counts.TryGetValue(key, out var result))
        return result;

    if (paths[node].Contains("out"))
    {
        var count = Convert.ToInt32(dac && fft);
        counts[key] = count;
        return count;
    }

    var sum = 0L;
    foreach (var newNode in paths[node])
    {
        sum += newNode switch
        {
            "dac" => CountPaths2(newNode, true, fft),
            "fft" => CountPaths2(newNode, dac, true),
            _ => CountPaths2(newNode, dac, fft)
        };
    }

    counts[key] = sum;
    return sum;
}
