namespace AdventOfCode.Y2023.Day19;

public class PartRanges
{
    public const int AbsoluteMinRange = 1;
    public const int AbsoluteMaxRange = 4000;

    public SimpleRange X { get; set; } = new();
    public SimpleRange M { get; set; } = new();
    public SimpleRange A { get; set; } = new();
    public SimpleRange S { get; set; } = new();

    public long RangesProduct => (long)X.Size * (long)M.Size * (long)A.Size * (long)S.Size;
}
