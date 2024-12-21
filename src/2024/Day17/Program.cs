#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var registerA = int.Parse(lines[0].Split(' ')[2]);
var registerB = int.Parse(lines[1].Split(' ')[2]);
var registerC = int.Parse(lines[2].Split(' ')[2]);
var instructions = lines[4].Split(' ')[1].Split(',').Select(int.Parse).ToArray();

var computer = new Computer(registerA, registerB, registerC, instructions);
computer.Run();

Console.WriteLine($"Part 1: {string.Join(',', computer.Outputs)}");

internal class Computer
{
    public List<int> Outputs { get; } = [];
    
    private int _registerA;
    private int _registerB;
    private int _registerC;
    private int _instructionPointer;

    private readonly int[] _instructions;
    private readonly Action<int>[] _operations;
    
    public Computer(int registerA, int registerB, int registerC, int[] instructions)
    {
        _registerA = registerA;
        _registerB = registerB;
        _registerC = registerC;
        _instructions = instructions;

        _operations =
        [
            Adv,
            Bxl,
            Bst,
            Jnz,
            Bxc,
            Out,
            Bdv,
            Cdv,
        ];
    }

    public void Run()
    {
        while (_instructionPointer < _instructions.Length - 1)
        {
            var instruction = _instructions[_instructionPointer++];
            var operand = _instructions[_instructionPointer++];
            _operations[instruction](operand);
        }
    }

    private void Adv(int operand)
    {
        _registerA = (int) (_registerA / Math.Pow(2, ToComboOperand(operand)));
    }

    private void Bxl(int operand)
    {
        _registerB ^= operand;
    }

    private void Bst(int operand)
    {
        _registerB = ToComboOperand(operand) % 8;
    }

    private void Jnz(int operand)
    {
        if (_registerA == 0)
            return;

        _instructionPointer = operand;
    }

    private void Bxc(int operand)
    {
        _registerB ^= _registerC;
    }

    private void Out(int operand)
    {
        Outputs.Add(ToComboOperand(operand) % 8);
    }

    private void Bdv(int operand)
    {
        _registerB = (int) (_registerA / Math.Pow(2, ToComboOperand(operand)));
    }

    private void Cdv(int operand)
    {
        _registerC = (int) (_registerA / Math.Pow(2, ToComboOperand(operand)));
    }

    private int ToComboOperand(int operand)
    {
        return operand switch
        {
            >= 0 and <= 3 => operand,
            4 => _registerA,
            5 => _registerB,
            6 => _registerC,
            _ => throw new IndexOutOfRangeException()
        };
    }
}
