namespace AdventOfCode.Y2023.Models;

[Flags]
public enum Direction : byte
{
    Undefined = 0,
    Same = 0,
    North = 1,
    West = 2,
    South = 4,
    East = 8,

    NorthWest = North | West,
    NorthEast = North | East,
    SouthWest = South | West,
    SouthEast = South | East,

    Invalid = 0xFF
}

public class DirectionException : Exception
{
    public DirectionException(string message, Exception innerException = null) : base(message, innerException) { }
}
