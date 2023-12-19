namespace AdventOfCode2023.Day19;

// values are inclusive: Min >= n <= Max
public class SimpleRange
{
    public int Min { get; set; }
    public int Max { get; set; }
    public int Size => Max - Min + 1; // e.g., 1415 - 100 + 1 = 1315

    public SimpleRange()
    {
        Min = PartRanges.AbsoluteMinRange;
        Max = PartRanges.AbsoluteMaxRange;
    }

    public SimpleRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString() => $"{Min} >= n <= {Max}";
}
