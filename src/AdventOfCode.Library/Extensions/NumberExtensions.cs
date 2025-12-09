using System.Numerics;

namespace AdventOfCode.Library.Extensions;

public static class NumberExtensions
{
    extension<TNumber>(TNumber number)
        where TNumber : INumber<TNumber>
    {
        public TNumber Gcd(TNumber other)
        {
            while (true)
            {
                if (number == TNumber.Zero)
                    return other;
                var a1 = number;
                number = other % number;
                other = a1;
            }
        }

        public TNumber Mod(TNumber other)
        {
            return (number % other + other) % other;
        }
    }
}
