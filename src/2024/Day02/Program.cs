#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var reports = lines.Select(l => l.Split(' ').Select(int.Parse).ToList()).ToList();

var part1Count = 0;
var part2Count = 0;

foreach (var report in reports)
{
    var (isSaveForPart1, isSaveForPart2) = CheckReport(report);

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

static (bool IsSaveForPart1, bool IsSaveForPart2) CheckReport(List<int> report)
{
    var (isSaveForPart1, unsafeIndex) = IsSave(report);
    if (isSaveForPart1)
    {
        return (true, true);
    }

    for (var i = unsafeIndex > 0 ? unsafeIndex - 1 : unsafeIndex; i <= unsafeIndex + 1; i++)
    {
        List<int> reportToTest = [..report[..i], ..report[(i + 1)..]];
        var (isSaveForPart2, _) = IsSave(reportToTest);
        if (isSaveForPart2)
        {
            return (false, true);
        }
    }

    return (false, false);
}

static (bool, int) IsSave(List<int> report)
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
    
    for (var i = 0; i < report.Count - 1; i++)
    {
        var diff = report[i + 1] - report[i];
        if (diff < start || diff > end)
        {
            return (false, i + 1);
        }
    }

    return (true, -1);
}
