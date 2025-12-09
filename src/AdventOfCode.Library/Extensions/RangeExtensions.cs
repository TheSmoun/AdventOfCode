namespace AdventOfCode.Library.Extensions;

public static class RangeExtensions
{
    extension(Range range)
    {
        public bool Contains(Range other)
        {
            return range.Start.Value <= other.Start.Value && range.End.Value >= other.End.Value;
        }

        public bool Overlaps(Range other)
        {
            return range.Start.Value <= other.Start.Value && range.End.Value > other.Start.Value 
                   || range.End.Value >= other.End.Value && range.Start.Value < other.End.Value;
        }
    }
}
