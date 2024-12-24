using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var wires = new ConcurrentDictionary<string, Wire>();
var gates = new ConcurrentDictionary<string, LogicGate>();

var i = 0;
string line;
while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var parts = line.Split(": ");
    wires[parts[0]] = new Wire(parts[0], int.Parse(parts[1]));
}

while (i < lines.Length && (line = lines[i++]) != string.Empty)
{
    var (leftInput, gate, rightInput, output) = Utils.ParseGate(line);
    
    var leftInputWire = wires.GetOrAdd(leftInput, w => new Wire(leftInput));
    var rightInputWire = wires.GetOrAdd(rightInput, w => new Wire(rightInput));
    var outputWire = wires.GetOrAdd(output, w => new Wire(output));

    LogicGate logicGate = gate switch
    {
        "AND" => new AndGate(leftInputWire, rightInputWire, outputWire),
        "OR" => new OrGate(leftInputWire, rightInputWire, outputWire),
        "XOR" => new XorGate(leftInputWire, rightInputWire, outputWire),
        _ => throw new UnreachableException()
    };
    
    gates[output] = logicGate;
}

var part1 = 0L;
i = 0;
while (wires.TryGetValue('z' + i.ToString().PadLeft(2, '0'), out var wire))
{
    var value = wire.GetOrComputeValue();
    part1 |= value << i;
    i++;
}

Console.WriteLine($"Part 1: {part1}");

internal class Wire(string name, long? value = null)
{
    public string Name { get; } = name;
    public long? Value { get; set; } = value;
    
    public LogicGate? Source { get; set; }
    public List<LogicGate> Sinks { get; } = [];

    public long GetOrComputeValue()
    {
        EnsureValue();
        return Value.GetValueOrDefault();
    }

    internal void EnsureValue()
    {
        if (Value.HasValue)
            return;

        if (Source is null)
            throw new UnreachableException();
        
        Source.CalculateOutput();
    }
}

internal abstract class LogicGate
{
    public Wire LeftInput { get; }
    public Wire RightInput { get; }
    public Wire Output { get; }
    
    protected LogicGate(Wire leftInput, Wire rightInput, Wire output)
    {
        LeftInput = leftInput;
        RightInput = rightInput;
        Output = output;
        
        LeftInput.Sinks.Add(this);
        RightInput.Sinks.Add(this);
        Output.Source = this;
    }
    
    public void CalculateOutput()
    {
        LeftInput.EnsureValue();
        RightInput.EnsureValue();
        CalculateOutputInternal();
    }
    
    protected abstract void CalculateOutputInternal();
}

internal class AndGate(Wire leftInput, Wire rightInput, Wire output) : LogicGate(leftInput, rightInput, output)
{
    protected override void CalculateOutputInternal()
    {
        Output.Value = LeftInput.Value & RightInput.Value;
    }
}

internal class OrGate(Wire leftInput, Wire rightInput, Wire output) : LogicGate(leftInput, rightInput, output)
{
    protected override void CalculateOutputInternal()
    {
        Output.Value = LeftInput.Value | RightInput.Value;
    }
}

internal class XorGate(Wire leftInput, Wire rightInput, Wire output) : LogicGate(leftInput, rightInput, output)
{
    protected override void CalculateOutputInternal()
    {
        Output.Value = LeftInput.Value ^ RightInput.Value;
    }
}

internal static partial class Utils
{
    [GeneratedRegex(@"^([a-zA-Z0-9]{3})\s(AND|XOR|OR)\s([a-zA-Z0-9]{3})\s->\s([a-zA-Z0-9]{3})$")]
    internal static partial Regex GateMatcher();

    public static (string LeftInput, string Gate, string RightInput, string Output) ParseGate(string line)
    {
        var match = GateMatcher().Match(line);
        var leftInput = match.Groups[1].Value;
        var gate = match.Groups[2].Value;
        var rightInput = match.Groups[3].Value;
        var output = match.Groups[4].Value;
        return (leftInput, gate, rightInput, output);
    }
}
