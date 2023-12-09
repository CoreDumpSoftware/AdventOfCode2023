using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day3;

public class SchematicValue
{
    public List<ValuePosition<char>> ValuePositions { get; set; } = new();
    public int Value { get; set; } = 0;
    public ValuePosition<char>? Symbol { get; set; } = null!;
    public bool IsPartNumber => Symbol != null;

    public override string ToString() => $"{Value}:{IsPartNumber}:{Symbol?.ToString() ?? "N/A"}";
}