namespace AoC.Shared.Lib;

public class Graph<T>
    where T : notnull
{
    public List<GraphNode<T>> Nodes { get; }
    public Dictionary<T, GraphNode<T>> NodesByValue { get; }

    public Graph(List<GraphNode<T>> nodes)
    {
        Nodes = nodes;
        NodesByValue = Nodes.ToDictionary(n => n.Value);
    }

    public GraphNode<T> GetNodeByValue(T value)
    {
        return NodesByValue[value];
    }
}
