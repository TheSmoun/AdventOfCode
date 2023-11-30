using AoC.Shared.Extensions;

namespace AoC.Shared;

public abstract class DayBase
{
    public abstract void Run();
}

public abstract class DayBase<TInput, TResult> : DayBase
{
    private const string InputsFolderName = "Inputs";

    protected abstract string Name { get; }
    
    private readonly string _inputFileName;

    protected DayBase()
    {
        _inputFileName = Path.Combine(InputsFolderName, $"{GetType().Name}.txt");
    }

    protected DayBase(string inputFileName)
    {
        _inputFileName = inputFileName;
    }

    public override void Run()
    {
        var lines = ReadInput();
        
        Console.WriteLine();
        ConsoleExtensions.WriteColoredLine($"   {Name}", ConsoleColor.Magenta);
        Console.WriteLine();
        
        RunPart(1, RunPart1, lines);
        RunPart(2, RunPart2, lines);
    }

    private string[] ReadInput()
    {
        return File.ReadAllLines(_inputFileName);
    }

    private void RunPart(int number, Func<TInput, TResult> partFunction, IEnumerable<string> lines)
    {
        var inputFunction = ParseInput;

        var (input, timeInput) = inputFunction.MeasureWith(lines);
        var (result, timeResult) = partFunction.MeasureWith(input);
        
        Console.Write($"-> Part {number}: ");
        ConsoleExtensions.WriteColoredLine($"{result}", ConsoleColor.Cyan);
        Console.WriteLine($"   Input: {timeInput.Format()}, Part {number}: {timeResult.Format()}, Total: {(timeInput + timeResult).Format()}");
        Console.WriteLine();
    }

    protected abstract TInput ParseInput(IEnumerable<string> lines);
    protected abstract TResult RunPart1(TInput input);
    protected abstract TResult RunPart2(TInput input);
}
