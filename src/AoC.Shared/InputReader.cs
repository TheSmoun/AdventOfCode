namespace AoC.Shared;

public interface IInputReader
{
    string[] ReadLines(string name);
}

public sealed class FileInputReader : IInputReader
{
    public string[] ReadLines(string name)
    {
        return File.ReadAllLines(name);
    }
}

public sealed class ConsoleInputReader : IInputReader
{
    public string[] ReadLines(string _)
    {
        using var streamReader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
        return streamReader.ReadToEnd().Split('\n');
    }
}
