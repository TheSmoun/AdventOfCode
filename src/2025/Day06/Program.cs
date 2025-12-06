using System.Diagnostics;
using MoreLinq;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var numbers = new List<long[]>();
for (var i = 0; i < lines.Length - 1; i++)
{
    numbers.Add(lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray());
}

var operators = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToList();

var mathProblems = new List<MathProblem>();

for (var i = 0; i < numbers[0].Length; i++)
{
    var nums = numbers.Select(n => n[i]).ToArray();

    switch (operators[i])
    {
        case '+':
            mathProblems.Add(new AddProblem(nums));
            break;
        case '*':
            mathProblems.Add(new MulProblem(nums));
            break;
    }
}

var sum = 0L;
foreach (var mathProblem in mathProblems)
{
    sum += mathProblem.Solve();
}

Console.WriteLine("Part 1: " + sum);

var columns = new List<string>();

for (var i = 0; i < lines[0].Length; i++)
{
    columns.Add(new string(lines.Select(l => l[i]).ToArray()));
}

var chunks = columns.Split(string.IsNullOrWhiteSpace).Select(c => c.ToList()).ToList();

mathProblems = [];

foreach (var chunk in chunks)
{
    var nums = new long[chunk.Count];
    var op = chunk[0][^1];
    
    nums[0] = long.Parse(chunk[0][..^1]);
    for (var i = 1; i < chunk.Count; i++)
    {
        nums[i] = long.Parse(chunk[i]);
    }
    
    switch (op)
    {
        case '+':
            mathProblems.Add(new AddProblem(nums));
            break;
        case '*':
            mathProblems.Add(new MulProblem(nums));
            break;
    }
}

sum = 0L;
foreach (var mathProblem in mathProblems)
{
    sum += mathProblem.Solve();
}

Console.WriteLine("Part 2: " + sum);

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

internal abstract class MathProblem(long[] numbers)
{
    protected readonly long[] Numbers = numbers;

    public abstract long Solve();
}

internal class AddProblem(long[] numbers) : MathProblem(numbers)
{
    public override long Solve()
    {
        var sum = 0L;
        foreach (var number in Numbers)
        {
            sum += number;
        }
        
        return sum;
    }
}

internal class MulProblem(long[] numbers) : MathProblem(numbers)
{
    public override long Solve()
    {
        var product = 1L;
        foreach (var number in Numbers)
        {
            product *= number;
        }
        
        return product;
    }
}
