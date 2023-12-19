using AdventOfCode2023.Models;

namespace AdventOfCode2023.Extensions;

public static class PositionExtensions
{
    public static int GetDistanceTo(this Position src, Position dst) =>
        Math.Abs(dst.X - src.X) + Math.Abs(dst.Y - src.Y);

    public static Position Next(this Position p, Direction d, int magnitude = 1) => d switch
    {
        Direction.North => new Position(p.X, p.Y - magnitude),
        Direction.West => new Position(p.X - magnitude, p.Y),
        Direction.South => new Position(p.X, p.Y + magnitude),
        Direction.East => new Position(p.X + magnitude, p.Y),
        Direction.NorthWest => new Position(p.X - magnitude, p.Y - magnitude),
        Direction.NorthEast => new Position(p.X + magnitude, p.Y - magnitude),
        Direction.SouthWest => new Position(p.X - magnitude, p.Y + magnitude),
        Direction.SouthEast => new Position(p.X + magnitude, p.Y + magnitude),
        _ => throw new ArgumentException($"Invalid direction."),
    };
}