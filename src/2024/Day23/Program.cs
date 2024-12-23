using System.Collections.Concurrent;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var graph = new ConcurrentDictionary<string, HashSet<string>>();

foreach (var line in lines)
{
    var parts = line.Split("-");
    graph.GetOrAdd(parts[0], []).Add(parts[1]);
    graph.GetOrAdd(parts[1], []).Add(parts[0]);
}

var cliques = new List<HashSet<string>>();
BronKerbosch([], [..graph.Keys], []);

var part1 = cliques
    .Where(c => c.Count >= 3)
    .SelectMany(c => c.ThreePermutations())
    .Where(p => p.Any(c => c[0] == 't'))
    .Select(c => c.ToTuple())
    .Distinct()
    .Count();

Console.WriteLine($"Part 1: {part1}");
    
var part2 = string.Join(',', cliques.MaxBy(c => c.Count)!.Order());
Console.WriteLine($"Part 2: {part2}");

return;

void BronKerbosch(HashSet<string> r, HashSet<string> p, HashSet<string> x)
{
    if (p.Count == 0 && x.Count == 0)
    {
        cliques.Add(r);
        return;
    }

    var u = p.Concat(x).First();
    foreach (var v in p.Except(graph[u]))
    {
        BronKerbosch([..r, v], [..p.Intersect(graph[v])], [..x.Intersect(graph[v])]);
        
        p.Remove(v);
        x.Add(v);
    }
}

internal static class HashSetExtensions
{
    public static IEnumerable<HashSet<string>> ThreePermutations(this HashSet<string> set)
    {
        foreach (var a in set)
        {
            foreach (var b in set)
            {
                foreach (var c in set)
                {
                    var permutation = new HashSet<string> { a, b, c };
                    if (permutation.Count == 3)
                        yield return permutation;
                }
            }
        }
    }

    public static (string, string, string) ToTuple(this HashSet<string> set)
    {
        var list = set.Order().ToList();
        return (list[0], list[1], list[2]);
    }
}
