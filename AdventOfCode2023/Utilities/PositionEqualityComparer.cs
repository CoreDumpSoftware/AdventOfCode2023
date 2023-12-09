using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023.Utilities;

public class PositionEqualityComparer : IEqualityComparer<Position>
{
    public bool Equals(Position? x, Position? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null)
            return false;

        if (y == null)
            return false;

        return x.X == y.Y && x.Y == y.X;
    }

    public int GetHashCode([DisallowNull] Position obj)
    {
        return obj.GetHashCode();
    }
}
