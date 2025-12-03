using System.Diagnostics;

Console.WriteLine("GO");

#if DEBUG
var lines = File.ReadAllLines("input.txt");
var start = Stopwatch.GetTimestamp();
#else
    using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
    var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var banks = lines.Select(l => l.ToCharArray().Select(c => c - '0').ToArray()).ToList();

Console.WriteLine("Part 1: " + GetTotalJoltage(2));
Console.WriteLine("Part 2: " + GetTotalJoltage(12));

#if DEBUG
var elapsedTime = Stopwatch.GetElapsedTime(start);
Console.WriteLine("End: " + elapsedTime);
#endif

long GetTotalJoltage(int digits)
{
    var sum = 0L;
    
    foreach (var bank in banks)
    {
        var joltage = 0L;
        var searchStart = 0;

        for (var d = digits - 1; d >= 0; d--)
        {
            var maxDigit = 0L;
            var maxIndex = -1;
        
            for (var i = searchStart; i < bank.Length - d; i++)
            {
                var digit = bank[i];
                if (digit > maxDigit)
                {
                    maxDigit = digit;
                    maxIndex = i;
                }
            }

            joltage += maxDigit * Math.Max((long)Math.Pow(10L, d), 1);
            searchStart = maxIndex + 1;
        }

        sum += joltage;
    }

    return sum;
}
