using AdventOfCode.Y2023.Models;

namespace AdventOfCode.Y2023.Day02;

public struct CubeCount
{
    public WriteOnce<int> Red { get; set; }
    public WriteOnce<int> Green { get; set; }
    public WriteOnce<int> Blue { get; set; }
}
