using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day2;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "2_1.txt";

    public override long PartOne()
    {
        var data = GetFileContents(SolutionInput);
        var gameIdRegex = new Regex(@"Game (?'Id'\d+)");
        var colorRegex = new Regex(@"(((?'Red' \d+) red)|((?'Blue' \d+) blue)|((?'Green' \d+) green))");

        var sum = 0;
        var redThreshold = 12;
        var greenThreshold = 13;
        var blueThreshold = 14;

        foreach (var line in data)
        {
            var gameId = int.Parse(gameIdRegex.Match(line).Groups["Id"].Value);
            var colorMatches = colorRegex.Matches(line);

            var redValues = GetColorValues(colorMatches, "Red");
            var greenValues = GetColorValues(colorMatches, "Green");
            var blueValues = GetColorValues(colorMatches, "Blue");

            var redValid = redValues.All(v => v <= redThreshold);
            var greenValid = greenValues.All(v => v <= greenThreshold);
            var blueValid = blueValues.All(v => v <= blueThreshold);

            if (redValid && greenValid && blueValid)
                sum += gameId;
        }

        return sum;
    }

    private static int[] GetColorValues(MatchCollection matches, string color) => matches
        .Select(x => x.Groups[color].Value)
        .Where(x => !string.IsNullOrEmpty(x))
        .Select(int.Parse)
        .ToArray();

    public override long PartTwo()
    {
        var data = GetFileContents(SolutionInput);
        var gameIdRegex = new Regex(@"Game (?'Id'\d+)");
        var colorRegex = new Regex(@"(((?'Red' \d+) red)|((?'Blue' \d+) blue)|((?'Green' \d+) green))");

        var sum = 0;

        foreach (var line in data)
        {
            var gameId = int.Parse(gameIdRegex.Match(line).Groups["Id"].Value);
            var colorMatches = colorRegex.Matches(line);

            var redValues = GetColorValues(colorMatches, "Red");
            var greenValues = GetColorValues(colorMatches, "Green");
            var blueValues = GetColorValues(colorMatches, "Blue");

            var maxRed = redValues.Max();
            var maxGreen = greenValues.Max();
            var maxBlue = blueValues.Max();

            var cube = maxRed * maxGreen * maxBlue;
            sum += cube;
        }

        return sum;
    }
}
