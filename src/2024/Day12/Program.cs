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
var areaId = 0;

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

var part2 = areas.Sum(a =>
{
    var area = a.Count;
    var fences = GetFenceCount(a);
    return area * fences;
});

Console.WriteLine($"Part 2: {part2}");

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
        var currentAreaId = areaId++;
        
        Tile nextTile;
        do
        {
            nextTile = queue.Pop();
            nextTile.InQueue = false;
            nextTile.AreaId = currentAreaId;
            area.Add(nextTile);
        } while (nextTile != tile);
        
        areas.Add(area);
    }
}

int GetFenceCount(List<Tile> area)
{
    var id = area[0].AreaId;
    var minX = area.Min(x => x.X);
    var maxX = area.Max(x => x.X);
    var minY = area.Min(x => x.Y);
    var maxY = area.Max(x => x.Y);

    var fences = 0;
    var current = -1;
    
    for (var y = minY; y <= maxY; y++)
    {
        for (var x = minX; x <= maxX; x++)
        {
            var myId = GetId(x, y);
            var topId = GetId(x, y - 1);

            if (myId != id || topId == id)
            {
                current = -1;
                continue;
            }

            if (y == current)
                continue;
            
            fences++;
            current = y;
        }
    }

    current = -1;
    
    for (var y = minY; y <= maxY; y++)
    {
        for (var x = minX; x <= maxX; x++)
        {
            var myId = GetId(x, y);
            var bottomId = GetId(x, y + 1);

            if (myId != id || bottomId == id)
            {
                current = -1;
                continue;
            }

            if (y == current)
                continue;
            
            fences++;
            current = y;
        }
    }

    current = -1;
    
    for (var x = minX; x <= maxX; x++)
    {
        for (var y = minY; y <= maxY; y++)
        {
            var myId = GetId(x, y);
            var leftId = GetId(x - 1, y);

            if (myId != id || leftId == id)
            {
                current = -1;
                continue;
            }

            if (x == current)
                continue;
            
            fences++;
            current = x;
        }
    }

    current = -1;
    
    for (var x = minX; x <= maxX; x++)
    {
        for (var y = minY; y <= maxY; y++)
        {
            var myId = GetId(x, y);
            var rightId = GetId(x + 1, y);

            if (myId != id || rightId == id)
            {
                current = -1;
                continue;
            }

            if (x == current)
                continue;
            
            fences++;
            current = x;
        }
    }

    return fences;

    int GetId(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return -1;
        return map[y, x].AreaId;
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
    public int AreaId { get; set; }

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
