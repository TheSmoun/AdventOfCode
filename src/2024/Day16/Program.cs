#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var height = lines.Length;
var width = lines[0].Length;
var horizontalMaze = new int[height, width];
var verticalMaze = new int[height, width];
var start = new Position(new Vec2(0, 0), Facing.Horizontal);
var end = new Vec2(0, 0);

for (var y = 0; y < height; y++)
{
    var line = lines[y];
    
    for (var x = 0; x < width; x++)
    {
        switch (line[x])
        {
            case '.':
                horizontalMaze[y, x] = int.MaxValue - 1;
                verticalMaze[y, x] = int.MaxValue - 1;
                break;
            case 'S':
                start = new Position(new Vec2(x, y), Facing.Horizontal);
                horizontalMaze[y, x] = 0;
                verticalMaze[y, x] = int.MaxValue - 1;
                break;
            case 'E':
                end = new Vec2(x, y);
                horizontalMaze[y, x] = int.MaxValue - 1;
                verticalMaze[y, x] = int.MaxValue - 1;
                break;
            default:
                horizontalMaze[y, x] = int.MaxValue;
                verticalMaze[y, x] = int.MaxValue;
                break;
        }
    }
}

var part1 = 0;
var minCosts = new List<(int Cost, Position Position)> { (0, start) };
var previous = new Dictionary<Position, List<Position>>();

while (minCosts.Count > 0)
{
    var item = minCosts.MinBy(e => e.Cost);
    var (cost, position) = item;
    minCosts.Remove(item);
    
    if (position.Coordinates.Equals(end))
    {
        part1 = cost;
        break;
    }

    foreach (var move in position.GetPossibleMoves())
    {
        var maze = move.Position.Facing == Facing.Horizontal ? horizontalMaze : verticalMaze;
        var currentDistance = maze[move.Position.Coordinates.Y, move.Position.Coordinates.X];
        if (currentDistance == int.MaxValue)
            continue;

        if (currentDistance > cost + move.Cost)
        {
            minCosts.Remove((currentDistance, move.Position));
            maze[move.Position.Coordinates.Y, move.Position.Coordinates.X] = cost + move.Cost;
            minCosts.Add((cost + move.Cost, move.Position));
            previous[move.Position] = [position];
        }
        else if (currentDistance == cost + move.Cost)
        {
            previous[move.Position].Add(position);
        }
    }
}

Console.WriteLine($"Part 1: {part1}");

var bestPositions = new HashSet<Position>();
var queue = new Queue<Position>();
queue.Enqueue(new Position(end, Facing.Horizontal));
queue.Enqueue(new Position(end, Facing.Vertical));

while (queue.Count > 0)
{
    var current = queue.Dequeue();

    if (bestPositions.Add(current))
    {
        if (previous.TryGetValue(current, out var previousPositions))
        {
            foreach (var previousPosition in previousPositions)
            {
                queue.Enqueue(previousPosition);
            }
        }
    }
}

var bestCoordinates = bestPositions.Select(p => p.Coordinates).ToHashSet();

Console.WriteLine($"Part 2: {bestCoordinates.Count}");

internal readonly struct Move : IEquatable<Move>
{
    public Position Position { get; }
    public int Cost { get; }

    public Move(Position position, int cost)
    {
        Position = position;
        Cost = cost;
    }

    public bool Equals(Move other)
    {
        return Position.Equals(other.Position) && Cost == other.Cost;
    }

    public override bool Equals(object? obj)
    {
        return obj is Move other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Cost);
    }
}

internal readonly struct Position : IEquatable<Position>
{
    public Vec2 Coordinates { get; }
    public Facing Facing { get; }

    public Position(Vec2 coordinates, Facing facing)
    {
        Coordinates = coordinates;
        Facing = facing;
    }

    public Move[] GetPossibleMoves()
    {
        var moves = new Move[3];

        if (Facing == Facing.Horizontal)
        {
            moves[0] = new Move(this + new Vec2(-1, 0), 1);
            moves[1] = new Move(this + new Vec2(1, 0), 1);
            moves[2] = new Move(new Position(Coordinates, Facing.Vertical), 1000);
        }
        else
        {
            moves[0] = new Move(this + new Vec2(0, -1), 1);
            moves[1] = new Move(this + new Vec2(0, 1), 1);
            moves[2] = new Move(new Position(Coordinates, Facing.Horizontal), 1000);
        }

        return moves;
    }

    public static Position operator +(Position a, Vec2 b)
    {
        return new Position(a.Coordinates + b, a.Facing);
    }

    public bool Equals(Position other)
    {
        return Coordinates.Equals(other.Coordinates) && Facing == other.Facing;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinates, (int)Facing);
    }
}

internal enum Facing : byte
{
    Horizontal = 0,
    Vertical = 1,
}

internal readonly struct Vec2 : IEquatable<Vec2>
{
    public int X { get; }
    public int Y { get; }

    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X + b.X, a.Y + b.Y);
    }

    public bool Equals(Vec2 other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
