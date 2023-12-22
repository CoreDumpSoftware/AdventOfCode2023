using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day19;

public class Part
{
    public int X { get; init; }
    public int M { get; init; }
    public int A { get; init; }
    public int S { get; init; }

    public int Sum => X + M + A + S;

    private Part() { }

    public static Part Parse(string line)
    {
        var variables = new int[4];
        var regex = new Regex(@"\d+");
        var index = 0;
        foreach (Match match in regex.Matches(line))
        {
            variables[index++] = int.Parse(match.Value);
        }

        return new()
        {
            X = variables[0],
            M = variables[1],
            A = variables[2],
            S = variables[3]
        };
    }
}
