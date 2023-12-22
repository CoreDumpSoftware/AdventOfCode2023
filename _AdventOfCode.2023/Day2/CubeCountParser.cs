using System.Text.RegularExpressions;
using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day2;

public class CubeCountParser
{
    private enum Color
    {
        Red,
        Green,
        Blue
    }

    private static readonly Regex CubeParser = new Regex(@"(?'Count'\d+) (?'Color'\w+)");
    private readonly StringReader _reader;

    public CubeCountParser(StringReader reader)
    {
        _reader = reader;
    }

    public IEnumerable<CubeCount> GetCubeCounts()
    {
        const char roundEnd = ';';
        const char roundSeparator = ',';
        const char endOfLine = '\0';

        var cubeCount = new CubeCount();
        var token = _reader.ReadUntil(roundSeparator, roundEnd);

        while (token.EndingCharacter != endOfLine)
        {
            var parts = token.Value.Split(' ');
            if (parts.Length != 2)
                throw new InvalidDataException($"Cube count token is malformed.");

            var count = int.Parse(parts[0]);
            var color = Enum.Parse<Color>(parts[1], true);

            switch(color)
            {
                case Color.Red:
                    cubeCount.Red = count;
                    break;
                case Color.Green:
                    cubeCount.Green = count;
                    break;
                case Color.Blue:
                    cubeCount.Blue = count;
                    break;
            }

            if (token.EndingCharacter == roundEnd || token.EndingCharacter == endOfLine)
            {
                yield return cubeCount;
                cubeCount = new();
            }

            token = _reader.ReadUntil(roundSeparator, roundEnd);
        }
    }
}
