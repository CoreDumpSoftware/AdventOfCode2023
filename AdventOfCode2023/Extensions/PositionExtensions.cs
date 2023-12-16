using AdventOfCode2023.Models;

namespace AdventOfCode2023.Extensions;

public static class PositionExtensions
{
    public static int GetDistanceTo(this Position src, Position dst) =>
        Math.Abs(dst.X - src.X) + Math.Abs(dst.Y - src.Y);

    public static Position Next(this Position p, Direction d) => d switch
    {
        Direction.North => new Position(p.X, p.Y - 1),
        Direction.West => new Position(p.X - 1, p.Y),
        Direction.South => new Position(p.X, p.Y + 1),
        Direction.East => new Position(p.X + 1, p.Y),
        Direction.NorthWest => new Position(p.X - 1, p.Y - 1),
        Direction.NorthEast => new Position(p.X + 1, p.Y - 1),
        Direction.SouthWest => new Position(p.X - 1, p.Y + 1),
        Direction.SouthEast => new Position(p.X + 1, p.Y + 1),
        _ => throw new ArgumentException($"Invalid direction."),
    };
}