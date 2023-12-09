namespace AdventOfCode2023.Utilities;

public class ValuePosition(char value, int x, int y) : Position(x, y)
{
    public char Value { get; set; } = value;

    public override string ToString() => $"{base.ToString()}: {Value}";
}
