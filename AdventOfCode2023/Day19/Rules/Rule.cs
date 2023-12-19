namespace AdventOfCode2023.Day19.Rules;

public class Rule
{
    protected readonly string? _operator;

    public string? Operator
    {
        get => _operator;
        init
        {
            _operator = value;
            if (_operator != null)
            {
                _compareFn = _operator == "<"
                    ? LessThan
                    : GreaterThan;
            }
        }
    }
    public int? Amount { get; init; }

    protected Func<int, bool> _compareFn = _ => true;

    // When the condition is met, execute the next rule
    public string TargetName { get; init; }

    public virtual bool Check(Part input) => true;

    private bool LessThan(int value) => value < Amount;
    private bool GreaterThan(int value) => value > Amount;

    public override string ToString() => GetType() == typeof(Rule) ? TargetName : GetExpressionString();

    protected virtual string _condition { get; set; } = null!;
    protected virtual string GetExpressionString() => $"{_condition.ToLower()}{_operator}{Amount}";

    public virtual void UpdateRanges(PartRanges ranges, bool isTrue)
    {
    }
}
