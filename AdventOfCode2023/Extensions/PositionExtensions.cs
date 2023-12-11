using AdventOfCode2023.Models;

namespace AdventOfCode2023.Extensions;

public static class PositionExtensions
{
    public static int GetDistanceTo(this Position src, Position dst) =>
        Math.Abs(dst.X - src.X) + Math.Abs(dst.Y - src.Y);
}