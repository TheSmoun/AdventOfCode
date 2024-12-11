using System.Runtime.InteropServices;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var stones = lines[0].Split(' ').Select(long.Parse).ToDictionary(l => l, _ => 1L);

for (var i = 0; i < 25; i++)
{
    Evolve();
}

Console.WriteLine($"Part 1: {GetCount()}");

for (var i = 0; i < 50; i++)
{
    Evolve();
}

Console.WriteLine($"Part 2: {GetCount()}");

return;

void Evolve()
{
    var newStones = new Dictionary<long, long>();

    foreach (var (stone, count) in stones)
    {
        if (stone == 0)
        {
            Increment(1, count);
        }
        else
        {
            var s = stone.ToString();
            if (s.Length % 2 == 0)
            {
                Increment(long.Parse(s[..(s.Length / 2)]), count);
                Increment(long.Parse(s[(s.Length / 2)..]), count);
            }
            else
            {
                Increment(stone * 2024, count);
            }
        }
    }

    stones = newStones;
    
    return;

    void Increment(long stone, long amount)
    {
        CollectionsMarshal.GetValueRefOrAddDefault(newStones, stone, out _) += amount;
    }
}

long GetCount()
{
    return stones.Sum(e => e.Value);
}
