Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var leftNumbers = lines.Select(l => int.Parse(l.AsSpan().Slice(0, 5))).ToList();
leftNumbers.Sort();

var rightNumbers = lines.Select(l => int.Parse(l.AsSpan().Slice(8, 5))).ToList();
rightNumbers.Sort();

var difference = 0;
for (var i = 0; i < leftNumbers.Count; i++)
{
    difference += Math.Abs(rightNumbers[i] - leftNumbers[i]);
}

Console.WriteLine("Part 1: " + difference);

var rightNumberCount = new Dictionary<int, int>();
foreach (var rightNumber in rightNumbers)
{
    rightNumberCount[rightNumber] = rightNumberCount.GetValueOrDefault(rightNumber, 0) + 1;
}

var similarity = 0;
foreach (var leftNumber in leftNumbers)
{
    similarity += leftNumber * rightNumberCount.GetValueOrDefault(leftNumber, 0);
}

Console.WriteLine("Part 2: " + similarity);
