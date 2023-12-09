namespace AdventOfCode2023.Models;

public class Range<T>(T start, T end)
    where T : struct, IComparable, IComparable<T>, IEquatable<T>, IConvertible, IFormattable
{
    public T Start { get; set; } = start;
    public T End { get; set; } = end;
    public long Length { get; set; }

    public bool Contains(T value) => Start.CompareTo(value) >= 0 && End.CompareTo(value) == -1;

    public override string ToString() =>
        $"({start} >= n < {end})";
}