namespace AdventOfCode2023.Models;

public class Position(int x, int y)
{
    public int X { get; init; } = x;
    public int Y { get; init; } = y;

    public override bool Equals(object? other)
    {
        if (other == null || other is not Position p)
            return false;

        return base.Equals(other)
            || X == p.X && Y == p.Y;
    }

    public override string ToString() => $"({X}, {Y})";

    public override int GetHashCode() => Y * X + X; // Converts a 2 dimensional position to a unique linear index value

    public static bool operator ==(Position? left, Position? right)
    {
        if (left == null! && right == null!)
            return true;

        if (left == null! || right == null!)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Position? left, Position? right)
    {
        return !(left == right);
    }
}
