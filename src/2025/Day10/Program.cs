using System.Diagnostics;
using Microsoft.Z3;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var machines = lines.Select(l => new Machine(l)).ToArray();

var totalPaths = 0;
foreach (var machine in machines)
{
    var paths = new Dictionary<string, int>();
    var queue = new Queue<string>();

    var initialState = new string('.', machine.Target.Length);
    paths[initialState] = 0;
    queue.Enqueue(initialState);

    while (queue.Count > 0)
    {
        var state = queue.Dequeue();
        if (state == machine.Target)
        {
            totalPaths += paths[state];
            break;
        }

        foreach (var button in machine.Buttons)
        {
            var newState = string.Join("", state.Select((c, i) =>
            {
                if (button.Any(b => b == i))
                {
                    return c == '#' ? '.' : '#';
                }
                return c;
            }));
            
            if (!paths.ContainsKey(newState))
            {
                paths.Add(newState, paths[state] + 1);
                queue.Enqueue(newState);
            }
        }
    }
}

Console.WriteLine("Part 1: " + totalPaths);

var sum = 0L;
foreach (var machine in machines)
{
    sum += machine.Solve();
}

Console.WriteLine("Part 2: " + sum);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

internal class Machine
{
    public string Target { get; }
    public int[][] Buttons { get; }
    public int[] Joltages { get; }
    
    public Machine(string line)
    {
        var parts = line.Split(' ');
        var lightsPart = parts[0].Substring(1, parts[0].Length - 2);
        
        Target = lightsPart;
        
        Buttons = new int[parts.Length - 2][];
        
        for (var i = 1; i < parts.Length - 1; i++)
        {
            var buttonPart = parts[i].Substring(1, parts[i].Length - 2);
            Buttons[i - 1] = buttonPart.Split(',').Select(int.Parse).ToArray();
        }
        
        var joltagePart = parts[^1].Substring(1, parts[^1].Length - 2);
        Joltages = joltagePart.Split(',').Select(int.Parse).ToArray();
    }

    public long Solve()
    {
        var context = new Context();
        var optimize = context.MkOptimize();

        var buttonPresses = new IntExpr[Buttons.Length];
        for (var i = 0; i < Buttons.Length; i++)
        {
            var press = context.MkIntConst($"p{i}");
            buttonPresses[i] = press;
            optimize.Add(context.MkGe(press, context.MkInt(0)));
        }

        for (var i = 0; i < Joltages.Length; i++)
        {
            var affectingPresses = buttonPresses.Where((_, j) => Buttons[j].Contains(i)).ToArray();
            if (affectingPresses.Length > 0)
            {
                var sum = affectingPresses.Length == 1 ? affectingPresses[0] : context.MkAdd(affectingPresses);
                optimize.Add(context.MkEq(sum, context.MkInt(Joltages[i])));
            }
        }

        optimize.MkMinimize(buttonPresses.Length == 1 ? buttonPresses[0] : context.MkAdd(buttonPresses));
        optimize.Check();

        var model = optimize.Model;
        return buttonPresses.Sum(b => ((IntNum)model.Evaluate(b, true)).Int64);
    }
}
