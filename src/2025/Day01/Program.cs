Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var instructions = lines.Select(l =>
{
    var direction = l[0] == 'L' ? -1 : 1;
    var value = int.Parse(l[1..]);
    return direction * value;
}).ToList();

const int size = 100;
var dial = 50;
var zeros = 0;

foreach (var instruction in instructions)
{
    dial = Mod(dial + instruction, size);
    if (dial == 0)
        zeros++;
}

Console.WriteLine("Part 1: " + zeros);

dial = 50;
zeros = 0;

foreach (var instruction in instructions)
{
    var direction = Math.Sign(instruction);
    var target = dial + instruction;

    for (var i = dial; i != target; i += direction)
    {
        if (i % size == 0)
        {
            zeros++;
        }
    }
    
    dial = Mod(target, size);
}

Console.WriteLine("Part 2: " + zeros);

return;

static int Mod(int x, int m)
{
    return (x % m + m) % m;
}
