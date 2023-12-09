namespace AdventOfCode2023.Day9;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "9_1.txt";
    protected override string PartTwoInputFile { get; init; } = "9_1_sample.txt";

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile);

        var report = data.Select(l => new ReportLine(l)).ToList();
        var extrapolationSums = report.Select(l => l.ExtrapolateForward()).Sum();

        return extrapolationSums;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile);

        var report = data.Select(l => new ReportLine(l)).ToList();
        var extrapolationSums = report.Select(l => l.ExtrapolateBackward()).Sum();

        return extrapolationSums;
    }
}
