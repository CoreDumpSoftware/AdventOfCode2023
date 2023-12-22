namespace AdventOfCode.Y2023.Day19.Rules;

public class MRule : Rule
{
    protected override string _condition { get; set; } = "M";

    public override bool Check(Part input) => _compareFn(input.M);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.M.Max > Amount - 1)
            {
                ranges.M.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.M.Min < Amount + 1)
            {
                ranges.M.Min = Amount.Value + 1;
            }
        }
        else
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.M.Min < Amount)
            {
                ranges.M.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.M.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.M.Max = Amount.Value;
            }
        }
    }
}
