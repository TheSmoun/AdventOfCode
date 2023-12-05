using AoC.Shared;
using MoreLinq;

namespace AoC._2023.Days;

public sealed class Day05 : DayBase<List<Day05.SeedMap>, long>
{
    protected override string Name => "Day 5: If You Give A Seed A Fertilizer";

    protected override List<SeedMap> ParseInput(IEnumerable<string> lines)
    {
        var parts = lines.Split(string.Empty).ToList();
        var seeds = parts[0]
            .First()[7..]
            .Split(' ')
            .Select(s => new SeedMap(long.Parse(s)))
            .ToList();
        
        ApplyMap(seeds, parts[1], s => s.Seed, s => s.Soil, (s, l) => s.Soil = l);
        ApplyMap(seeds, parts[2], s => s.Soil, s => s.Fertilizer, (s, l) => s.Fertilizer = l);
        ApplyMap(seeds, parts[3], s => s.Fertilizer, s => s.Water, (s, l) => s.Water = l);
        ApplyMap(seeds, parts[4], s => s.Water, s => s.Light, (s, l) => s.Light = l);
        ApplyMap(seeds, parts[5], s => s.Light, s => s.Temperature, (s, l) => s.Temperature = l);
        ApplyMap(seeds, parts[6], s => s.Temperature, s => s.Humidity, (s, l) => s.Humidity = l);
        ApplyMap(seeds, parts[7], s => s.Humidity, s => s.Location, (s, l) => s.Location = l);

        return seeds;
    }

    protected override long RunPart1(List<SeedMap> input)
        => input.Min(s => s.Location);

    protected override long RunPart2(List<SeedMap> input)
    {
        return 0;
    }

    private static void ApplyMap(List<SeedMap> seeds, IEnumerable<string> map, Func<SeedMap, long> sourceGetter,
        Func<SeedMap, long> targetGetter, Action<SeedMap, long> setter)
    {
        foreach (var seed in seeds)
        {
            setter(seed, sourceGetter(seed));
        }
        
        foreach (var mapRow in map.Skip(1))
        {
            var mapRowNumbers = mapRow.Split(' ').Select(long.Parse).ToArray();
            var destinationStart = mapRowNumbers[0];
            var sourceStart = mapRowNumbers[1];
            var length = mapRowNumbers[2];

            foreach (var seed in seeds)
            {
                var source = sourceGetter(seed);
                var target = targetGetter(seed);
                if (sourceStart > source || sourceStart + length < source || source != target)
                    continue;
                
                var diff = source - sourceStart;
                setter(seed, destinationStart + diff);
            }
        }
    }
    
    public class SeedMap
    {
        public long Seed { get; }
        public long Soil { get; set; }
        public long Fertilizer { get; set; }
        public long Water { get; set; }
        public long Light { get; set; }
        public long Temperature { get; set; }
        public long Humidity { get; set; }
        public long Location { get; set; }
        
        public SeedMap(long seed)
        {
            Seed = seed;
        }
    }
}
