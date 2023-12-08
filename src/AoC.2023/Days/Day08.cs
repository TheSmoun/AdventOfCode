using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.Extensions;
using AoC.Shared.Lib;
using MoreLinq;

namespace AoC._2023.Days;

public partial class Day08 : DayBase<Day08.Input, long>
{
    protected override string Name => "Day 8: Haunted Wasteland";

    private readonly Regex _regex = GetRegex();
    
    protected override Input ParseInput(IEnumerable<string> lines)
    {
        var parts = lines.Split(string.Empty).ToList();
        var instructions = parts[0].First().ToList();

        var nodes = new List<GraphNode<string>>();
        var source2Target = new Dictionary<string, (string, string)>();
        var source2Node = new Dictionary<string, GraphNode<string>>();
        
        foreach (var line in parts[1])
        {
            var match = _regex.Match(line);
            var source = match.Groups["source"].Value;
            var left = match.Groups["left"].Value;
            var right = match.Groups["right"].Value;
            
            var node = new GraphNode<string>(source);
            nodes.Add(node);
            source2Target.Add(source, (left, right));
            source2Node.Add(source, node);
        }

        foreach (var node in nodes)
        {
            var (left, right) = source2Target[node.Value];
            var leftNode = source2Node[left];
            var rightNode = source2Node[right];
            
            node.Next.Add(leftNode);
            leftNode.Prev.Add(node);
            
            node.Next.Add(rightNode);
            rightNode.Prev.Add(node);
        }

        return new Input
        {
            Instructions = instructions,
            Graph = new Graph<string>(nodes),
        };
    }

    protected override long RunPart1(Input input)
    {
        var count = 0;
        var node = input.Graph.GetNodeByValue("AAA");
        
        while (node.Value != "ZZZ")
        {
            var instruction = input.Instructions[count % input.Instructions.Count];
            var index = instruction == 'L' ? 0 : 1;
            node = node.Next[index];
            count++;
        }

        return count;
    }

    protected override long RunPart2(Input input)
    {
        return input.Graph
            .GetAllMatchingNodes(n => n.Value[2] == 'A')
            .Select(n => RunForNode(n, input.Instructions))
            .Lcm();
    }

    private long RunForNode(GraphNode<string> node, List<char> instructions)
    {
        var count = 0;
        while (node.Value[2] != 'Z')
        {
            var instruction = instructions[count % instructions.Count];
            var index = instruction == 'L' ? 0 : 1;
            node = node.Next[index];
            count++;
        }

        return count;
    }

    public class Input
    {
        public required List<char> Instructions { get; init; }
        public required Graph<string> Graph { get; init; }
    }

    [GeneratedRegex(@"^(?<source>[A-Z]{3})\s=\s\((?<left>[A-Z]{3}),\s(?<right>[A-Z]{3})\)$")]
    private static partial Regex GetRegex();
}
