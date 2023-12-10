using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day10;

public static class DirectionExtensions
{
    public static bool IsOppositeOrSame(this Direction direction) => direction switch
    {
        Direction.North or Direction.South => direction == Direction.North || direction == Direction.South,
        Direction.West or Direction.East => direction == Direction.West || direction == Direction.East,
        _ => true
    };

    public static Direction GetDirectionTo(this Position src, Position dst)
    {
        Direction result = 0;

        if (src.X != dst.X)
            result |= src.X > dst.X ? Direction.West : Direction.East;

        if (dst.Y != src.Y)
            result |= src.Y > dst.Y ? Direction.North : Direction.South;

        return result;
    }
}
