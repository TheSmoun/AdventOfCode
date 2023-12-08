namespace AoC.Shared.Lib;

public class GraphNode<T>
{
    public T Value { get; }
    
    public List<GraphNode<T>> Next { get; }
    public List<GraphNode<T>> Prev { get; }

    public GraphNode(T value)
    {
        Value = value;
        Next = new List<GraphNode<T>>();
        Prev = new List<GraphNode<T>>();
    }
}
