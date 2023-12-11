using System.Numerics;

namespace AoC.Shared.Math;

public readonly struct Vec2<TNumber> : IEquatable<Vec2<TNumber>>
    where TNumber : INumber<TNumber>
{
    public TNumber X { get; }
    public TNumber Y { get; }

    public Vec2(TNumber x, TNumber y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Vec2<TNumber> left, Vec2<TNumber> right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Vec2<TNumber> left, Vec2<TNumber> right)
    {
        return !(left == right);
    }

    public static Vec2<TNumber> operator +(Vec2<TNumber> left, Vec2<TNumber> right)
    {
        return new Vec2<TNumber>(left.X + right.X, left.Y + right.Y);
    }

    public static Vec2<TNumber> operator -(Vec2<TNumber> left, Vec2<TNumber> right)
    {
        return new Vec2<TNumber>(left.X - right.X, left.Y - right.Y);
    }

    public bool Equals(Vec2<TNumber> other)
    {
        return EqualityComparer<TNumber>.Default.Equals(X, other.X)
               && EqualityComparer<TNumber>.Default.Equals(Y, other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec2<TNumber> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"<{X}, {Y}>";
    }
}
