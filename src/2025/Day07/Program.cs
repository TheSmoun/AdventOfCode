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

var timelines = new long[lines[0].Length];

var firstLine = lines[0];
for (var i = 0; i < firstLine.Length; i++)
{
    var c = firstLine[i];
    if (c == 'S')
    {
        timelines[i] = 1;
    }
}

var splits = 0;
for (var i = 2; i < lines.Length; i += 2)
{
    var line = lines[i];
    for (var t = 0; t < timelines.Length; t++)
    {
        var timeline = timelines[t];
        if (timeline == 0)
            continue;
        
        if (line[t] == '^')
        {
            splits++;
            timelines[t - 1] += timeline;
            timelines[t + 1] += timeline;
            timelines[t] = 0;
        }
    }
}

Console.WriteLine("Part 1: " + splits);
Console.WriteLine("Part 2: " + timelines.Sum());

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif
