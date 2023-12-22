using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Models;

public class Position(int x, int y)
{
    public static implicit operator Position (string input) => Parse(input);

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
        var leftNull = ReferenceEquals(left, null);
        var rightNull = ReferenceEquals(right, null);

        if (leftNull && rightNull)
            return true;

        if (leftNull || rightNull)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Position? left, Position? right)
    {
        return !(left == right);
    }

    public static Position Parse(string input)
    {
        var match = Regex.Match(input, @"\((?'X'\d+), (?'Y'\d+)\)");

        if (!match.Success)
            throw new ArgumentException($"Malformed position string: \"{input}\"");

        return new(
            int.Parse(match.Groups["X"].Value),
            int.Parse(match.Groups["Y"].Value)
        );
    }
}
