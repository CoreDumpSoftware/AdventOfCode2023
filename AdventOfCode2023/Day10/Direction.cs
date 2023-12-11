namespace AdventOfCode2023.Day10;

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

    Invalid= 0xFF
}
