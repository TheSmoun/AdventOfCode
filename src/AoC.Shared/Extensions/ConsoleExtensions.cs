namespace AoC.Shared.Extensions;

public static class ConsoleExtensions
{
    public static void WriteColoredLine(string line, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(line);
        Console.ResetColor();
    }
}
