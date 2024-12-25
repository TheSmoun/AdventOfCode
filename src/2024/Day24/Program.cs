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

    LogicGate _ = gate switch
    {
        "AND" => new AndGate(leftInputWire, rightInputWire, outputWire),
        "OR" => new OrGate(leftInputWire, rightInputWire, outputWire),
        "XOR" => new XorGate(leftInputWire, rightInputWire, outputWire),
        _ => throw new UnreachableException()
    };
}

var device = new MonitoringDevice(wires);

Console.WriteLine($"Part 1: {device.GetValue('z')}");

for (var bit = 0; bit < 45; bit++)
{
    foreach (var wire in wires.Values)
    {
        if (wire.Name[0] == 'x' || wire.Name[0] == 'y')
        {
            wire.Value = 0;
        }
        else
        {
            wire.Value = null;
        }
    }

    var x = 1L << bit;
    var y = 1L << bit;
    var z = x + y;

    wires[Wire.ConstructName('x', bit)].Value = 1;
    wires[Wire.ConstructName('y', bit)].Value = 1;
    
    var result = device.GetValue('z');
    if (z != result)
    {
        Console.WriteLine($"Mismatch at bit {bit}");
    }
}

internal class MonitoringDevice
{
    private readonly ConcurrentDictionary<string, Wire> _wires;

    public MonitoringDevice(ConcurrentDictionary<string, Wire> wires)
    {
        _wires = wires;
    }
    
    public long GetValue(char prefix)
    {
        var value = 0L;
        var i = 0;
        
        while (_wires.TryGetValue(Wire.ConstructName(prefix, i), out var wire))
        {
            var bit = wire.GetOrComputeValue();
            value |= bit << i;
            i++;
        }

        return value;
    }
}

internal class Wire(string name, long? value = null)
{
    public string Name { get; } = name;
    public long? Value { get; set; } = value;
    
    public LogicGate? Source { get; set; }

    public static string ConstructName(char prefix, int bit)
    {
        return prefix + bit.ToString().PadLeft(2, '0');
    }
    
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

    public override string ToString()
    {
        return Name;
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
