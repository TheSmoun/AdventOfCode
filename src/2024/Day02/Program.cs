#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var reports = lines.Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();

var part1Count = 0;
var part2Count = 0;

for (var i = 0; i < reports.Length; i++)
{
    var (isSaveForPart1, isSaveForPart2) = CheckReport(reports[i]);

    if (isSaveForPart1)
    {
        part1Count++;
    }

    if (isSaveForPart2)
    {
        part2Count++;
    }
}

Console.WriteLine("Part 1: " + part1Count);
Console.WriteLine("Part 2: " + part2Count);

return;

static (bool IsSaveForPart1, bool IsSaveForPart2) CheckReport(int[] report)
{
    int start;
    int end;

    if (report[0] < report[^1])
    {
        start = 1;
        end = 3;
    }
    else
    {
        start = -3;
        end = -1;
    }
    
    var (isSaveForPart1, unsafeIndex) = IsSave(report, start, end, -1);
    if (isSaveForPart1)
    {
        return (true, true);
    }

    for (var i = Math.Max(0, unsafeIndex - 1); i <= unsafeIndex + 1; i++)
    {
        var (isSaveForPart2, _) = IsSave(report, start, end, i);
        if (isSaveForPart2)
        {
            return (false, true);
        }
    }

    return (false, false);
}

static (bool, int) IsSave(int[] report, int start, int end, int skip)
{
    var prev = report[skip == 0 ? 1 : 0];
    
    for (var i = skip == 0 ? 2 : 1; i < report.Length; i++)
    {
        if (i == skip)
            continue;
        
        var curr = report[i];
        var diff = curr - prev;
        
        if (diff < start || diff > end)
        {
            return (false, i);
        }
        
        prev = curr;
    }

    return (true, -1);
}
