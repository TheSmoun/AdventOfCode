using System.Diagnostics;

#if DEBUG
    var lines = File.ReadAllLines("input.txt");
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().Split('\n');
#endif

var sum = lines.Where(l => l.Length > 0).Sum(l =>
{
    var firstDigit = l.SkipWhile(c => !char.IsDigit(c)).First() - '0';
    var lastDigit = l.Reverse().SkipWhile(c => !char.IsDigit(c)).First() - '0';
    return firstDigit * 10 + lastDigit;
});

Console.WriteLine("Part 1: " + sum);

var dict = new Dictionary<string, int>
{
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 }
};

sum = lines.Where(l => l.Length > 0).Sum(l =>
{
    var firstDigit = FindFirstDigit(l, 0, 1, dict);
    var lastDigit = FindFirstDigit(l, l.Length - 1, -1, dict);
    return firstDigit * 10 + lastDigit;
});

Console.WriteLine("Part 2: " + sum);

return;

static int FindFirstDigit(string line, int firstIndex, int nextIndexOffset, Dictionary<string, int> dict)
{
    for (var i = firstIndex; i >= 0 && i < line.Length; i += nextIndexOffset)
    {
        if (char.IsDigit(line[i]))
            return line[i] - '0';

        var part = line[i..];
            
        foreach (var (key, value) in dict)
        {
            if (part.StartsWith(key))
                return value;
        }
    }

    throw new UnreachableException();
}
