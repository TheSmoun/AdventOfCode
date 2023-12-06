using AoC.Shared;
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
            .ToList();

        var seedRanges = parts[0]
            .First()[7..]
            .Split(' ')
            .Chunk(2)
            .Select(c => new SeedRange
            {
                Start = long.Parse(c[0]),
                Length = long.Parse(c[1]),
            })
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
                
                map.Add(new MapEntry
                {
                    DestinationStart = destinationStart,
                    SourceStart = sourceStart,
                    Length = length,
                });
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
        => input.Process(input.Seeds, false).Min();

    protected override long RunPart2(Almanac input)
    {
        var location = 0L;
        while (true)
        {
            var seed = input.Process(location, true);
            if (input.SeedRanges.Any(r => r.Contains(seed)))
                break;

            location++;
        }
        
        return location;
    }

    public class Almanac
    {
        public required List<long> Seeds { get; init; }
        public required List<SeedRange> SeedRanges { get; init; }
        public required List<Map> Maps { get; init; }

        public List<long> Process(List<long> seeds, bool invert)
            => seeds.Select(s => Process(s, invert)).ToList();

        public long Process(long seed, bool invert)
            => Maps.Aggregate(seed, (l, map) => map.Apply(l, invert));
    }

    public class SeedRange
    {
        public required long Start { get; init; }
        public required long Length { get; init; }

        public bool Contains(long seed)
        {
            return seed >= Start && seed <= Start + Length;
        }
    }

    public class Map : List<MapEntry>
    {
        public long Apply(long seed, bool invert)
        {
            return this
                .Select(e => e.Transform(seed, invert))
                .FirstOrDefault(v => v.HasValue) ?? seed;
        }
    }

    public class MapEntry
    {
        public required long DestinationStart { get; init; }
        public required long SourceStart { get; init; }
        public required long Length { get; init; }
        
        public long? Transform(long input, bool invert)
        {
            if (invert)
                return Transform(input, DestinationStart, SourceStart, Length);
            
            return Transform(input, SourceStart, DestinationStart, Length);
        }

        private static long? Transform(long input, long sourceStart, long destinationStart, long length)
        {
            if (input >= sourceStart && input <= sourceStart + length)
                return destinationStart + input - sourceStart;

            return null;
        }
    }
}
