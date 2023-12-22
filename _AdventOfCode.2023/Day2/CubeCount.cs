using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day2;

public struct CubeCount
{
    public WriteOnce<int> Red { get; set; }
    public WriteOnce<int> Green { get; set; }
    public WriteOnce<int> Blue { get; set; }
}
