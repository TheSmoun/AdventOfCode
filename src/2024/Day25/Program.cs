#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var keys = new List<Key>();
var locks = new List<Lock>();

var i = 0;
while (i < lines.Length)
{
    string line;
    var j = 0;
    var isLock = false;
    var heights = new int[] { 7, 7, 7, 7, 7 };
    var offset = -1;
    
    while (i < lines.Length && (line = lines[i++]) != string.Empty)
    {
        if (j == 0)
        {
            isLock = line[0] == '#';
            heights = [-1, -1, -1, -1, -1];
            offset = 1;
        }

        for (var k = 0; k < line.Length; k++)
        {
            if (line[k] == '#')
            {
                heights[k] += offset;
            }
        }
        
        j++;
    }

    if (isLock)
    {
        locks.Add(new Lock(heights));
    }
    else
    {
        keys.Add(new Key(heights));
    }
}

var part1 = 0;

foreach (var key in keys)
{
    foreach (var @lock in locks)
    {
        if (key.Heights.Zip(@lock.Heights).Select(tuple => tuple.First + tuple.Second).All(sum => sum <= 5))
        {
            part1++;
        }
    }
}

Console.WriteLine($"Part 1: {part1}");

internal record Key(int[] Heights);
internal record Lock(int[] Heights);
