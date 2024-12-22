#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var height = lines.Length;
var width = lines[0].Length;

var map = new Tile[height, width];

for (var y = 0; y < height; y++)
{
    var line = lines[y];

    for (var x = 0; x < width; x++)
    {
        map[y, x] = new Tile(x, y, line[x], map, width, height);
    }
}

var index = 0;
var queue = new Stack<Tile>();
var areas = new List<List<Tile>>();

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        var tile = map[y, x];
        if (tile.Index == int.MaxValue)
        {
            StrongConnect(tile);
        }
    }
}

var part1 = areas.Sum(a =>
{
    var area = a.Count;
    var perimeter = a.Sum(t => 4 - t.GetNeighbours().Count);
    return area * perimeter;
});

Console.WriteLine($"Part 1: {part1}");

return;

void StrongConnect(Tile tile)
{
    tile.Index = index;
    tile.LowLink = index;

    index++;
    queue.Push(tile);
    tile.InQueue = true;

    foreach (var neighbour in tile.GetNeighbours())
    {
        if (neighbour.Index == int.MaxValue)
        {
            StrongConnect(neighbour);
            tile.LowLink = Math.Min(tile.LowLink, neighbour.LowLink);
        }
        else if (neighbour.InQueue)
        {
            tile.LowLink = Math.Min(tile.LowLink, neighbour.Index);
        }
    }

    if (tile.LowLink == tile.Index)
    {
        var area = new List<Tile>();

        Tile nextTile;
        do
        {
            nextTile = queue.Pop();
            nextTile.InQueue = false;
            area.Add(nextTile);
        } while (nextTile != tile);
        
        areas.Add(area);
    }
}

internal class Tile : IEquatable<Tile>
{
    public int X { get; }
    public int Y { get; }
    public char Name { get; }
    
    public int Index { get; set; }
    public int LowLink { get; set; }
    public bool InQueue { get; set; }

    private readonly Tile[,] _map;
    private readonly int _width;
    private readonly int _height;

    public Tile(int x, int y, char name, Tile[,] map, int width, int height)
    {
        X = x;
        Y = y;
        Name = name;
        Index = int.MaxValue;
        LowLink = int.MaxValue;
        InQueue = false;
        _map = map;
        _width = width;
        _height = height;
    }

    public static bool operator ==(Tile tile1, Tile tile2)
    {
        return tile1.X == tile2.X && tile1.Y == tile2.Y;
    }

    public static bool operator !=(Tile tile1, Tile tile2)
    {
        return !(tile1 == tile2);
    }

    public List<Tile> GetNeighbours()
    {
        var neighbours = new List<Tile>(4);
        
        TryAddNeighbour(X - 1, Y, neighbours);
        TryAddNeighbour(X + 1, Y, neighbours);
        TryAddNeighbour(X, Y - 1, neighbours);
        TryAddNeighbour(X, Y + 1, neighbours);
        
        return neighbours;
    }

    private void TryAddNeighbour(int x, int y, List<Tile> neighbours)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return;
        
        var tile = _map[y, x];
        if (tile.Name == Name)
        {
            neighbours.Add(tile);
        }
    }

    public bool Equals(Tile? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Tile)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
