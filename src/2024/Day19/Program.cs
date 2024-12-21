#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var towels = lines[0].Split(", ").ToHashSet();
var filteredTowels = new List<string>();

foreach (var towel in towels)
{
    var parts = towels.Where(t => t != towel).ToList();
    if (!IsPossible(towel, parts))
    {
        filteredTowels.Add(towel);
    }
}

var patterns = lines.Skip(2).ToArray();

Console.WriteLine($"Part 1: {patterns.Count(p => IsPossible(p, filteredTowels))}");

return;

bool IsPossible(ReadOnlySpan<char> pattern, List<string> parts)
{
    if (pattern.Length == 0)
        return true;
    
    foreach (var towel in parts)
    {
        if (pattern.StartsWith(towel) && IsPossible(pattern[towel.Length..], parts))
            return true;
    }
    
    return false;
}
