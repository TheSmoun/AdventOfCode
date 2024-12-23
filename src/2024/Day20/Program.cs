#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var height = lines.Length;
var width = lines[0].Length;

var map = new int[height, width];
(int Y, int X) start = (0, 0);
(int Y, int X) end = (0, 0);

var cheats = new List<(int Y, int X)>();

for (var y = 0; y < height; y++)
{
    var line = lines[y];
    
    for (var x = 0; x < width; x++)
    {
        switch (line[x])
        {
            case '#':
                map[y, x] = int.MaxValue;
                if (y > 0 && y < height - 1 && x > 0 && x < width - 1)
                {
                    cheats.Add((y, x));
                }
                break;
            case '.':
                map[y, x] = int.MaxValue - 1;
                break;
            case 'S':
                map[y, x] = 0;
                start = (y, x);
                break;
            case 'E':
                map[y, x] = int.MaxValue - 1;
                end = (y, x);
                break;
        }
    }
}

PerformAStar();

var part1 = 0;
foreach (var (y, x) in cheats)
{
    if (TestCheat(y, x))
    {
        part1++;
    }
}

Console.WriteLine($"Part 1: {part1}");

var raceTrack = new List<(int Y, int X, int Distance)>();
for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        if (map[y, x] != int.MaxValue)
        {
            raceTrack.Add((y, x, map[y, x]));
        }
    }
}

var part2 = 0;
for (var y = 0; y < raceTrack.Count; y++)
{
    var (y0, x0, d0) = raceTrack[y];
    
    for (var x = 0; x < raceTrack.Count; x++)
    {
        if (x == y)
            continue;

        var (y1, x1, d1) = raceTrack[x];
        var distance = Math.Abs(y1 - y0) + Math.Abs(x1 - x0);
        if (distance <= 20 && d1 - d0 - distance >= 100)
        {
            part2++;
        }
    }
}

Console.WriteLine($"Part 2: {part2}");

return;

void PerformAStar()
{
    var queue = new HashSet<(int Y, int X)> { start };

    while (queue.Count > 0)
    {
        var current = queue.MinBy(e => map[e.Y, e.X]);
        if (current.Equals(end))
            break;
        
        queue.Remove(current);

        var neighbours = new (int Y, int X)[]
        {
            (current.Y + 1, current.X),
            (current.Y - 1, current.X),
            (current.Y, current.X + 1),
            (current.Y, current.X - 1),
        };

        var score = map[current.Y, current.X] + 1;
        foreach (var neighbour in neighbours)
        {
            if (neighbour.Y < 0 || neighbour.Y >= height || neighbour.X < 0 || neighbour.X >= width)
                continue;
            
            if (map[neighbour.Y, neighbour.X] == int.MaxValue)
                continue;
            
            if (score < map[neighbour.Y, neighbour.X])
            {
                map[neighbour.Y, neighbour.X] = score;
                queue.Add(neighbour);
            }
        }
    }
}

bool TestCheat(int y, int x)
{
    return TestCheatInternal(1, 0) || TestCheatInternal(0, 1);

    bool TestCheatInternal(int yOffset, int xOffset)
    {
        var score1 = map[y + yOffset, x + xOffset];
        var score2 = map[y - yOffset, x - xOffset];
        if (score1 == int.MaxValue || score2 == int.MaxValue)
            return false;
        
        return Math.Abs(score1 - score2) >= 102;
    }
}
