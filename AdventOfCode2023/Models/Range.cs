namespace AdventOfCode2023.Models;

public class Range(long start, long length)
{
    public long Start { get; init; } = start;
    public long End { get; init; } = start + length;
    public long Length { get; init; } = length;

    public bool Contains(long value) => Start <= value && End > value;
}
