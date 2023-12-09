namespace AdventOfCode2023.Utilities;

public class Position(int x, int y) : IEquatable<Position>
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public bool Equals(Position? other) =>
        other != null &&
        this == other ||
        X == other.X &&
        Y == other.Y;

    public override string ToString() => $"({X}, {Y})";
}
