using System.Diagnostics;
using AdventOfCode.Library.Math;
using NetTopologySuite.Geometries;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var redTiles = new List<Vec2<int>>();

foreach (var line in lines)
{
    var parts = line.Split(',');
    var x = int.Parse(parts[0]);
    var y = int.Parse(parts[1]);
    redTiles.Add(new Vec2<int>(x, y));
}

var largestArea = 0L;

for (var i = 0; i < redTiles.Count - 1; i++)
{
    for (var j = i + 1; j < redTiles.Count; j++)
    {
        largestArea = Math.Max(largestArea, GetArea(redTiles[i], redTiles[j]));
    }
}

Console.WriteLine("Part 1: " + largestArea);

largestArea = 0L;

var factory = new GeometryFactory();

redTiles.Add(redTiles[0]); // wrap around for polygon
var polygon = factory.CreatePolygon(redTiles.Select(v => new Coordinate(v.X, v.Y)).ToArray());
redTiles.RemoveAt(redTiles.Count - 1);

for (var i = 0; i < redTiles.Count - 1; i++)
{
    for (var j = i; j < redTiles.Count; j++)
    {
        var envelope = new Envelope(new Coordinate(redTiles[i].X, redTiles[i].Y), new Coordinate(redTiles[j].X, redTiles[j].Y));
        var rectangle = factory.ToGeometry(envelope);
        if (rectangle.Within(polygon))
        {
            largestArea = Math.Max(largestArea, GetArea(redTiles[i], redTiles[j]));
        }
    }
}

Console.WriteLine("Part 2: " + largestArea);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

long GetArea(Vec2<int> v0, Vec2<int> v1)
{
    var x = Math.Abs(v0.X - v1.X) + 1L;
    var y = Math.Abs(v0.Y - v1.Y) + 1L;
    return x * y;
}
