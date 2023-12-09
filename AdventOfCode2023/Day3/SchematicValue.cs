using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.Day3;

public class SchematicValue
{
    public List<ValuePosition> ValuePositions { get; set; } = new();
    public int Value { get; set; } = 0;
    public ValuePosition Symbol { get; set; }
    public bool IsPartNumber => Symbol != null;

    public override string ToString() => $"{Value}:{IsPartNumber}:{Symbol?.ToString() ?? "N/A"}";
}