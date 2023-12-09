namespace AdventOfCode2023.Day1;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "1_1.txt";
    protected override string PartTwoInputFile { get; init; } = null!;

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile);
        var sum = 0;

        foreach (var line in data)
        {
            var firstNumber = line.First(x => char.IsDigit(x));
            var lastNumber = line.Last(x => char.IsDigit(x));

            sum += int.Parse((new string(new[] { firstNumber, lastNumber} )));
        }

        return sum;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile);
        var sum = 0;

        foreach (var line in data)
        {
            var sanitized = Day1LineSanitizer.SanitizeLine(line);
            var firstNumber = sanitized.First(x => char.IsDigit(x));
            var lastNumber = sanitized.Last(x => char.IsDigit(x));

            sum += int.Parse((new string(new[] { firstNumber, lastNumber })));
        }

        return sum;
    }
}
