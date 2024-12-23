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

var triangles = new HashSet<(string, string, string)>();
foreach (var n1 in graph.Keys)
{
    foreach (var n2 in graph[n1])
    {
        foreach (var n3 in graph[n1].Intersect(graph[n2]))
        {
            var nodes = new List<string> { n1, n2, n3 };
            if (nodes.All(n => n[0] != 't'))
                continue;
            
            nodes.Sort();
            triangles.Add((nodes[0], nodes[1], nodes[2]));
        }
    }
}

Console.WriteLine($"Part 1: {triangles.Count}");

var cliques = new List<HashSet<string>>();
BronKerbosch([], [..graph.Keys], []);

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
