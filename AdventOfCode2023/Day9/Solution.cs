using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day9;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "9_1.txt";
    protected override string PartTwoInputFile { get; init; } = "9_1_sample.txt";

    private long Recurse(long[] values, bool forwards = true)
    {
        if (values.All(v => v == 0))
            return 0;

        var decompositions = new long[values.Length - 1];

        for (var i = 0; i < values.Length - 1; i++)
            decompositions[i] = values[i + 1] - values[i];

        var delta = Recurse(decompositions, forwards);

        return forwards
            ? values[values.Length - 1] + delta
            : values[0] - delta;
    }

    public override long PartOne()
    {
        var result = GetFileContents(PartOneInputFile)
            .Select(l => l.ParseLongs().ToArray())
            .Select(x => Recurse(x, true))
            .Sum();

        return result;
    }

    public override long PartTwo()
    {
        var result = GetFileContents(PartOneInputFile)
            .Select(l => l.ParseLongs().ToArray())
            .Select(x => Recurse(x, false))
            .Sum();

        return result;
    }
}
