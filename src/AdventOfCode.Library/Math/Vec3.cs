using System.Numerics;

namespace AdventOfCode.Library.Math;

public readonly struct Vec3<T>(T x, T y, T z) : IEquatable<Vec3<T>>, IAdditionOperators<Vec3<T>, Vec3<T>, Vec3<T>>
    where T : INumber<T>
{
    public T X { get; } = x;
    public T Y { get; } = y;
    public T Z { get; } = z;

    public static Vec3<T> operator +(Vec3<T> a, Vec3<T> b)
    {
        return new Vec3<T>(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public bool Equals(Vec3<T> other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec3<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
