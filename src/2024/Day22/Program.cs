using System.Runtime.InteropServices;
using MoreLinq;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var seeds = lines.Select(long.Parse).ToArray();
var part1 = seeds.Select(s => CalcSecretNumbers(s).Last()).Sum();

Console.WriteLine($"Part 1: {part1}");

var allSequenceResults = new Dictionary<(int, int, int, int), int>();
foreach (var seed in seeds)
{
    var sequenceResults = new Dictionary<(int, int, int, int), int>();
    foreach (var prices in CalcSecretNumbers(seed).Select(p => (int) p % 10).Window(5))
    {
        var diff1 = prices[1] - prices[0];
        var diff2 = prices[2] - prices[1];
        var diff3 = prices[3] - prices[2];
        var diff4 = prices[4] - prices[3];
        
        var diffs = (diff1, diff2, diff3, diff4);
        if (!sequenceResults.ContainsKey((diff1, diff2, diff3, diff4)))
        {
            sequenceResults[diffs] = prices[4];
        }
    }

    foreach (var (changes, maxPrice) in sequenceResults)
    {
        CollectionsMarshal.GetValueRefOrAddDefault(allSequenceResults, changes, out _) += maxPrice;
    }
}

var part2 = allSequenceResults.Values.Max();
Console.WriteLine($"Part 2: {part2}");

return;

IEnumerable<long> CalcSecretNumbers(long seed)
{
    var current = seed;
    yield return current;
    
    for (var i = 0; i < 2000; i++)
    {
        current = CalcNextSecretNumber(current);
        yield return current;
    }
}

long CalcNextSecretNumber(long secretNumber)
{
    secretNumber = (secretNumber ^ (secretNumber << 6)) % 16777216;
    secretNumber = (secretNumber ^ (secretNumber >> 5)) % 16777216;
    secretNumber = (secretNumber ^ (secretNumber << 11)) % 16777216;
    
    return secretNumber;
}
