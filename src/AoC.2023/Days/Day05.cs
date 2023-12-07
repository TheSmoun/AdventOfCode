using AoC.Shared;
using AoC.Shared.Extensions;
using MoreLinq;

namespace AoC._2023.Days;

public sealed class Day05 : DayBase<Day05.Almanac, long>
{
    protected override string Name => "Day 5: If You Give A Seed A Fertilizer";

    protected override Almanac ParseInput(IEnumerable<string> lines)
    {
        var parts = lines.Split(string.Empty).ToList();
        var seeds = parts[0]
            .First()[7..]
            .Split(' ')
            .Select(long.Parse)
            .Select(s => LongRange.FromStartAndEnd(s, s))
            .ToList();

        var seedRanges = parts[0]
            .First()[7..]
            .Split(' ')
            .Chunk(2)
            .Select(c => LongRange.FromStartAndLength(long.Parse(c[0]), long.Parse(c[1])))
            .ToList();

        var maps = new List<Map>();
        foreach (var mapPart in parts.Skip(1))
        {
            var map = new Map();
            maps.Add(map);

            foreach (var mapLine in mapPart.Skip(1))
            {
                var mapRowNumbers = mapLine.Split(' ').Select(long.Parse).ToArray();
                var destinationStart = mapRowNumbers[0];
                var sourceStart = mapRowNumbers[1];
                var length = mapRowNumbers[2];
                
                map.Add(new MapEntry(sourceStart, destinationStart, length));
            }
        }

        return new Almanac
        {
            Seeds = seeds,
            SeedRanges = seedRanges,
            Maps = maps,
        };
    }

    protected override long RunPart1(Almanac input)
        => input.Process(input.Seeds);

    protected override long RunPart2(Almanac input)
        => input.Process(input.SeedRanges);

    public class Almanac
    {
        public required List<LongRange> Seeds { get; init; }
        public required List<LongRange> SeedRanges { get; init; }
        public required List<Map> Maps { get; init; }

        public long Process(List<LongRange> ranges)
            => Maps
                .Aggregate(ranges, (r, m) => m.Apply(r))
                .Min(r => r.Start);
    }

    public class Map : List<MapEntry>
    {
        public List<LongRange> Apply(List<LongRange> ranges)
        {
            var queue = ranges.ToQueue();
            var result = new List<LongRange>();
            
            while (queue.Count > 0)
            {
                var range = queue.Dequeue();
                
                var mapped = false;
                foreach (var entry in this)
                {
                    var (min, max) = range.Intersect(entry.Source);
                    if (max <= min)
                        continue;

                    mapped = true;
                    result.Add(LongRange.FromStartAndLength(min - entry.Source.Start + entry.Destination.Start, max - min));

                    if (min > range.Start)
                        queue.Enqueue(LongRange.FromStartAndLength(range.Start, min - range.Start));
                    
                    if (max < range.Start + range.Length)
                        queue.Enqueue(LongRange.FromStartAndLength(max, range.Start + range.Length - max));

                    break;
                }
                
                if (!mapped)
                    result.Add(range);
            }

            return result;
        }
    }

    public class MapEntry
    {
        public LongRange Source { get; }
        public LongRange Destination { get; }

        public MapEntry(long sourceStart, long destinationStart, long length)
        {
            Source = LongRange.FromStartAndLength(sourceStart, length);
            Destination = LongRange.FromStartAndLength(destinationStart, length);
        }
    }

    public readonly struct LongRange
    {
        public long Start { get; }
        public long Length { get; }

        private LongRange(long start, long length)
        {
            Start = start;
            Length = length;
        }

        public static LongRange FromStartAndLength(long start, long length)
        {
            return new LongRange(start, length);
        }

        public static LongRange FromStartAndEnd(long start, long end)
        {
            return new LongRange(start, end - start + 1);
        }

        public (long, long) Intersect(LongRange range)
        {
            var min = Math.Max(Start, range.Start);
            var max = Math.Min(Start + Length, range.Start + range.Length);
            return (min, max);
        }
    }
}
