using AdventOfCode.Y2023.Models;

namespace AdventOfCode.Y2023.Day03;

public class SchematicValue
{
    public List<ValuePosition<char>> ValuePositions { get; set; } = new();
    public long Value { get; set; } = 0;
    public ValuePosition<char>? Symbol { get; set; } = null!;
    public bool IsPartNumber => Symbol != null;

    public override string ToString() => $"{Value}:{IsPartNumber}:{Symbol?.ToString() ?? "N/A"}";
}