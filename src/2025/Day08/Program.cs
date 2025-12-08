using System.Diagnostics;
using System.Runtime.InteropServices;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var positions = lines.Select(l =>
{
    var parts = l.Split(',');
    return new Vec3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
}).ToArray();

var distances = new List<Connection>();
for (var i = 0; i < positions.Length - 1; i++)
{
    var p0 = positions[i];

    for (var j = i + 1; j < positions.Length; j++)
    {
        var p1 = positions[j];
        distances.Add(new Connection(i, j, p0.GetDistance(p1)));
    }
}

distances.Sort();

var parents = new int[positions.Length];
for (var i = 0; i < positions.Length; i++)
{
    parents[i] = i;
}

for (var i = 0; i < 1000; i++)
{
    var connection = distances[i];
    Union(connection.FromIndex, connection.ToIndex);
}

var closest = new Dictionary<int, long>();
for (var i = 0; i < positions.Length; i++)
{
    CollectionsMarshal.GetValueRefOrAddDefault(closest, Find(i), out _) += 1;
}

var result = closest.Values.OrderDescending().Take(3).Aggregate((a, b) => a * b);

Console.WriteLine("Part 1: " + result);

parents = new int[positions.Length];
var ranks = new int[positions.Length];
var size = positions.Length;

for (var i = 0; i < positions.Length; i++)
{
    parents[i] = i;
}

result = 0L;
foreach (var connection in distances)
{
    UnionWithRank(connection.FromIndex, connection.ToIndex);

    if (size == 1)
    {
        result = positions[connection.FromIndex].X * positions[connection.ToIndex].X;
        break;
    }
}

Console.WriteLine("Part 2: " + result);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

return;

// https://en.wikipedia.org/wiki/Disjoint-set_data_structure
int Find(int i)
{
    while (true)
    {
        if (i == parents[i])
            return i;
        
        i = parents[i];
    }
}

void Union(int i, int j)
{
    parents[Find(i)] = Find(j);
}

void UnionWithRank(int i, int j)
{
    var iParent = Find(i);
    var jParent = Find(j);

    if (iParent == jParent)
        return;
    
    var iRank = ranks[iParent];
    var jRank = ranks[jParent];

    if (iRank > jRank)
    {
        parents[jParent] = iParent;
    }
    else if (iRank < jRank)
    {
        parents[iParent] = jParent;
    }
    else
    {
        parents[jParent] = iParent;
        ranks[iParent] += 1;
    }

    size--;
}

internal readonly struct Vec3 : IEquatable<Vec3>
{
    public long X { get; }
    public long Y { get; }
    public long Z { get; }

    public Vec3(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vec3 operator +(Vec3 a, Vec3 b)
    {
        return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public double GetDistance(Vec3 other)
    {
        var x = Math.Abs(X - other.X);
        var y = Math.Abs(Y - other.Y);
        var z = Math.Abs(Z - other.Z);
        return Math.Sqrt(x * x + y * y + z * z);
    }

    public bool Equals(Vec3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}

internal class Connection(int fromIndex, int toIndex, double distances) : IComparable<Connection>
{
    public int FromIndex { get; } = fromIndex;
    public int ToIndex { get; } = toIndex;
    public double Distance { get; } = distances;

    public int CompareTo(Connection? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Distance.CompareTo(other.Distance);
    }
}
