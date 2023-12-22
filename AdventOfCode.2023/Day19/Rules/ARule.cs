namespace AdventOfCode.Y2023.Day19.Rules;

public class ARule : Rule
{
    protected override string _condition { get; set; } = "A";

    public override bool Check(Part input) => _compareFn(input.A);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.A.Max > Amount - 1)
            {
                ranges.A.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.A.Min < Amount + 1)
            {
                ranges.A.Min = Amount.Value + 1;
            }
        }
        else
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.A.Min < Amount)
            {
                ranges.A.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.A.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.A.Max = Amount.Value;
            }
        }
    }
}
