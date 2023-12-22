namespace AdventOfCode.Y2023.Models;

public class ValuePosition<T>(T value, int x, int y) : Position(x, y)
{
    public static implicit operator T(ValuePosition<T> value) => value.Value;

    public T Value { get; set; } = value;

    public override string ToString() => $"{base.ToString()}: {Value}";
}
