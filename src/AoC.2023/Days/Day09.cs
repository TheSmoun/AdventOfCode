using AoC.Shared;
using MoreLinq;

namespace AoC._2023.Days;

public class Day09 : DayBase<List<Day09.Sequence>, long>
{
    protected override string Name => "Day 9: Mirage Maintenance";

    protected override List<Sequence> ParseInput(IEnumerable<string> lines)
        => lines.Select(l => new Sequence(l.Split(' ').Select(long.Parse))).ToList();

    protected override long RunPart1(List<Sequence> input)
    {
        var sum = 0L;
        foreach (var sequence in input)
        {
            sequence.Derive();
            sequence.Extrapolate();
            sum += sequence[^1];
        }

        return sum;
    }

    protected override long RunPart2(List<Sequence> input)
    {
        foreach (var sequence in input)
        {
            sequence.Reverse();
        }

        return RunPart1(input);
    }

    public class Sequence : List<long>
    {
        public Sequence? Child { get; private set; }
        
        public Sequence(IEnumerable<long> numbers) : base(numbers) { }

        public void Derive()
        {
            if (this.All(i => i == 0))
                return;
            
            Child = new Sequence(this.Window(2).Select(w => w[1] - w[0]));
            Child.Derive();
        }

        public void Extrapolate()
        {
            if (Child is null)
            {
                Add(0);
            }
            else
            {
                Child.Extrapolate();
                Add(this[^1] + Child[^1]);
            }
        }
    }
}
