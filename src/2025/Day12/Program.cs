using System.Diagnostics;
using System.Runtime.CompilerServices;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var count = 0;
for (var i = 30; i < lines.Length; i++)
{
    var line = lines[i].AsSpan();
    var width = Parse2DigitInt(line.Slice(0, 2));
    var height = Parse2DigitInt(line.Slice(3, 2));
    var area = width * height;

    var presentsArea = 0;
    for (var j = 7; j < line.Length; j += 3)
    {
        presentsArea += Parse2DigitInt(line.Slice(j, 2)) * 7;
    }

    if (presentsArea < area)
    {
        count++;
    }
}

Console.WriteLine("Part 1: " + count);
Console.WriteLine("Part 2: " + string.Empty);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

[MethodImpl(MethodImplOptions.AggressiveInlining)]
int Parse2DigitInt(ReadOnlySpan<char> s) => 10 * (s[0] - '0') + (s[1] - '0');
