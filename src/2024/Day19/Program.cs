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

var patterns = lines.Skip(2).Where(p => IsPossible(p, filteredTowels)).ToArray();
Console.WriteLine($"Part 1: {patterns.Length}");

var possibilities = new Dictionary<string, long>();
Console.WriteLine($"Part 2: {patterns.Sum(GetPossibilities)}");

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

long GetPossibilities(string pattern)
{
    if (possibilities.TryGetValue(pattern, out var count))
        return count;

    count = 0;
    foreach (var towel in towels)
    {
        if (pattern.Length == 0)
            return 1;
        
        if (pattern.StartsWith(towel))
            count += GetPossibilities(pattern[towel.Length..]);
    }
    
    possibilities[pattern] = count;
    return count;
}
