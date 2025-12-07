using System.Diagnostics;
using System.Runtime.InteropServices;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var startPos = 0;
var splitters = new List<List<int>>();

var firstLine = lines[0];
for (var x = 0; x < firstLine.Length; x++)
{
    var c = firstLine[x];
    if (c == 'S')
    {
        startPos = x;
    }
}

for (var y = 2; y < lines.Length; y += 2)
{
    var line = lines[y];
    var splittersOnLine = new List<int>();
    
    for (var x = 0; x < line.Length; x++)
    {
        var c = line[x];
        if (c == '^')
        {
            splittersOnLine.Add(x);
        }
    }
    
    splitters.Add(splittersOnLine);
}

var splits = 0;
var timelines = new Dictionary<int, long> { { startPos, 1 } };

foreach (var splittersOnLine in splitters)
{
    var newTimelines = new Dictionary<int, long>();
    
    foreach (var (beam, value) in timelines)
    {
        if (splittersOnLine.Contains(beam))
        {
            splits++;
            CollectionsMarshal.GetValueRefOrAddDefault(newTimelines, beam - 1, out _) += value;
            CollectionsMarshal.GetValueRefOrAddDefault(newTimelines, beam + 1, out _) += value;
        }
        else
        {
            CollectionsMarshal.GetValueRefOrAddDefault(newTimelines, beam, out _) += value;
        }
    }
    
    timelines = newTimelines;
}

Console.WriteLine("Part 1: " + splits);
Console.WriteLine("Part 2: " + timelines.Values.Sum());

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif
