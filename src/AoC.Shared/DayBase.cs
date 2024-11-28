using AoC.Shared.Extensions;

namespace AoC.Shared;

public abstract class DayBase
{
    public abstract void Run();
}

public abstract class DayBase<TInput, TResult, TInputReader> : DayBase
    where TInputReader : IInputReader, new()
{
    private const string InputsFolderName = "Inputs";

    protected abstract string Name { get; }
    
    private readonly string[] _lines;

    protected DayBase()
    {
        var inputFileName = Path.Combine(InputsFolderName, $"{GetType().Name}.txt");
        _lines = new TInputReader().ReadLines(inputFileName);
    }

    protected DayBase(string inputFileName)
    {
        _lines = new TInputReader().ReadLines(inputFileName);
    }

    public override void Run()
    {
        Console.WriteLine();
        ConsoleExtensions.WriteColoredLine($"   {Name}", ConsoleColor.Magenta);
        Console.WriteLine();
        
        RunPart(1, RunPart1, _lines);
        RunPart(2, RunPart2, _lines);
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

public abstract class DayBase<TInput, TResult> : DayBase<TInput, TResult, FileInputReader> {}

public abstract class DayBase<TResult> : DayBase<IEnumerable<string>, TResult>
{
    protected override IEnumerable<string> ParseInput(IEnumerable<string> lines) => lines;
}
