namespace AdventOfCode.Y2023.Day19.Rules;

public class XRule : Rule
{
    protected override string _condition { get; set; } = "X";

    public override bool Check(Part input) => _compareFn(input.X);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.X.Max > Amount - 1)
            {
                ranges.X.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.X.Min < Amount + 1)
            {
                ranges.X.Min = Amount.Value + 1;
            }
        }
        else // invert the operator
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.X.Min < Amount)
            {
                ranges.X.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.X.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.X.Max = Amount.Value;
            }
        }
    }
}
