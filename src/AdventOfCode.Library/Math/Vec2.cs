using System.Numerics;

namespace AdventOfCode.Library.Math;

public readonly struct Vec2<T>(T x, T y) : IEquatable<Vec2<T>>, IAdditionOperators<Vec2<T>, Vec2<T>, Vec2<T>>
    where T : INumber<T>
{
    public T X { get; } = x;
    public T Y { get; } = y;

    public static Vec2<T> operator +(Vec2<T> a, Vec2<T> b)
    {
        return new Vec2<T>(a.X + b.X, a.Y + b.Y);
    }

    public bool Equals(Vec2<T> other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec2<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
