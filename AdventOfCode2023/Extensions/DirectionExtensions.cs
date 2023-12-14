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

    public static Position ExtendByDirection(this Position input, Direction direction) => direction switch
    {
        Direction.North => new(input.X, input.Y - 1),
        Direction.South => new(input.X, input.Y + 1),
        Direction.West => new(input.X - 1, input.Y),
        Direction.East => new(input.X + 1, input.Y),
        _ => throw new DirectionException("Invalid direction to extend position.")
    };

    public static char GetExtensionPipePiece(this Direction direction) => direction switch
    {
        Direction.North or Direction.South => '|',
        Direction.West or Direction.East => '-',
        _ => throw new DirectionException("Invalid direction to get extension pipe piece.")
    };

    public static char GetFillPieceByDirection(this char c, Direction direction) => direction switch
    {
        Direction.North => c switch
        {
            'S' or 'J' or 'L' => '|',
            _ => ' '
        },
        Direction.South => c switch
        {
            'S' or 'F' or '7' => '|',
            _ => ' '
        },
        Direction.West => c switch
        {
            'S' or 'J' or '7' => '-',
            _ => ' '
        },
        Direction.East => c switch
        {
            'S' or 'L' or 'F' => '-',
            _ => ' '
        },
        _ => throw new ArgumentException()
    };
}
