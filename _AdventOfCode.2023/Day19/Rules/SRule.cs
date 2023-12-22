namespace AdventOfCode2023.Day19.Rules;

public class SRule : Rule
{
    protected override string _condition { get; set; } = "S";

    public override bool Check(Part input) => _compareFn(input.S);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            // s < 2000 --> 1..1999
            if (Operator == "<" && ranges.S.Max > Amount - 1)
            {
                ranges.S.Max = Amount.Value - 1;
            }
            // s > 2000 --> 2001..4000
            else if (Operator == ">" && ranges.S.Min < Amount + 1)
            {
                ranges.S.Min = Amount.Value + 1;
            }
        }
        else
        {
            // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.S.Min < Amount)
            {
                ranges.S.Min = Amount.Value;
            }
            // s > 1351 --> s <= 1351
            else if (Operator == ">" && ranges.S.Max > Amount)
            {
                ranges.S.Max = Amount.Value;
            }
        }
    }
}