#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

const int size = 71;
const int simulationSize = 1024;

var memory = new int[size, size];

for (var y = 0; y < size; y++)
{
    for (var x = 0; x < size; x++)
    {
        memory[y, x] = int.MaxValue - 1;
    }
}

memory[0, 0] = 0;

foreach (var line in lines.Take(simulationSize))
{
    ProcessLine(line);
}

Console.WriteLine($"Part 1: {GetStepCount()}");

var i = 1025;
var stepCount = 0;
var failingLine = string.Empty;

while (stepCount >= 0)
{
    var line = lines[i++];
    ProcessLine(line);
    stepCount = GetStepCount();
    if (stepCount < 0)
    {
        failingLine = line;
    }
}

Console.WriteLine($"Part 2: {failingLine}");

return;

void ProcessLine(string line)
{
    var parts = line.Split(',');
    var x = int.Parse(parts[0]);
    var y = int.Parse(parts[1]);
    memory[y, x] = int.MaxValue;
}

int GetStepCount()
{
    var scores = new int[size, size];
    Array.Copy(memory, scores, memory.Length);
    
    var queue = new HashSet<(int Y, int X)> { (0, 0) };
    var goal = (size - 1, size - 1);

    while (queue.Count > 0)
    {
        var current = queue.MinBy(e => scores[e.Y, e.X]);
        if (current.Equals(goal))
            return scores[goal.Item1, goal.Item2];
        
        queue.Remove(current);

        var neighbours = new (int Y, int X)[]
        {
            (current.Y + 1, current.X),
            (current.Y - 1, current.X),
            (current.Y, current.X + 1),
            (current.Y, current.X - 1),
        };

        var score = scores[current.Y, current.X] + 1;
        foreach (var neighbour in neighbours)
        {
            if (neighbour.Y < 0 || neighbour.Y >= size || neighbour.X < 0 || neighbour.X >= size)
                continue;
            
            if (scores[neighbour.Y, neighbour.X] == int.MaxValue)
                continue;
            
            if (score < scores[neighbour.Y, neighbour.X])
            {
                scores[neighbour.Y, neighbour.X] = score;
                queue.Add(neighbour);
            }
        }
    }

    return -1;
}
