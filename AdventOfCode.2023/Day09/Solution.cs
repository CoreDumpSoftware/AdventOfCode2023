using AdventOfCode.Y2023.Extensions;

namespace AdventOfCode.Y2023.Day09;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "9_1.txt";

    private long GetNextInSequence(long[] values)
    {
        if (values.All(v => v == 0))
            return 0;

        var decompositions = new long[values.Length - 1];

        for (var i = 0; i < values.Length - 1; i++)
            decompositions[i] = values[i + 1] - values[i];

        var delta = GetNextInSequence(decompositions);

        return values[values.Length - 1] + delta;
    }

    public override long PartOne()
    {
        var result = GetFileContents(SolutionInput)
            .Select(l => l.ParseLongs().ToArray())
            .Select(GetNextInSequence)
            .Sum();

        return result;
    }

    public override long PartTwo()
    {
        var result = GetFileContents(SolutionInput)
            .Select(l => l.ParseLongs().Reverse().ToArray())
            .Select(GetNextInSequence)
            .Sum();

        return result;
    }
}
