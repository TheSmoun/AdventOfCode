using System.Numerics;

namespace AdventOfCode.Library.Math;

public readonly struct Vec2<T>(T x, T y) : IVectorBase<Vec2<T>, T>
    where T : INumber<T>
{
    public T X { get; } = x;
    public T Y { get; } = y;

    public static Vec2<T> operator +(Vec2<T> a, Vec2<T> b)
    {
        return new Vec2<T>(a.X + b.X, a.Y + b.Y);
    }

    public static Vec2<T> operator -(Vec2<T> left, Vec2<T> right)
    {
        return new Vec2<T>(left.X - right.X, left.Y - right.Y);
    }

    public static bool operator ==(Vec2<T> left, Vec2<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vec2<T> left, Vec2<T> right)
    {
        return !(left == right);
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

    public override string ToString()
    {
        return  $"({X}, {Y})";
    }
}
