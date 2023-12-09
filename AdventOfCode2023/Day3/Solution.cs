using System.Text.RegularExpressions;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day3;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "3_1.txt";
    protected override string PartTwoInputFile { get; init; } = "3_1_sample.txt";

    private static IEnumerable<Position> GetAdjectPositions(
        Position position,
        int length,
        int dimensions)
    {
        if (position.X < 0 ||
            position.Y < 0 ||
            position.X >= dimensions ||
            position.Y >= dimensions)
            yield break;

        var leftAvailable = position.X > 0;
        var rightAvailable = position.X + length < dimensions - 1;
        var upAvailable = position.Y > 0;
        var downAvailable = position.Y < dimensions - 1;

        if (upAvailable)
        {
            if (leftAvailable) yield return new Position(position.X - 1, position.Y - 1);

            foreach (var i in Enumerable.Range(0, length))
                yield return new Position(position.X + i, position.Y - 1);

            if (rightAvailable)
                yield return new Position(position.X + length, position.Y - 1);
        }

        if (leftAvailable)
            yield return new Position(position.X - 1, position.Y);

        if (rightAvailable)
            yield return new Position(position.X + length, position.Y);

        if (downAvailable)
        {
            if (leftAvailable) yield return new Position(position.X - 1, position.Y + 1);

            foreach (var i in Enumerable.Range(0, length))
                yield return new Position(position.X + i, position.Y + 1);

            if (rightAvailable)
                yield return new Position(position.X + length, position.Y + 1);
        }
    }

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile);
        var regex = new Regex(@"\d+");
        var yIndex = 0;
        var matrix = new List<string>();
        var schematicValues = new List<SchematicValue>();

        foreach (var line in data)
            matrix.Add(line);

        var dimensions = matrix[0].Length;

        yIndex = 0;
        foreach (var line in matrix)
        {
            foreach (var match in regex.Matches(line).ToArray())
            {
                var value = int.Parse(match.Value);
                var adjacentPositions = GetAdjectPositions(new(match.Index, yIndex), match.Length, dimensions);
                ValuePosition<char> symbol = null!;

                foreach (var position in adjacentPositions)
                {
                    var c = matrix[position.Y][position.X];
                    if (c != '.' && !char.IsDigit(c))
                    {
                        symbol = new(c, position.X, position.Y);
                        break;
                    }
                }

                schematicValues.Add(new SchematicValue
                {
                    Value = value,
                    ValuePositions = Enumerable.Range(0, match.Length).Select(i => new ValuePosition<char>(matrix[yIndex][match.Index + i], match.Index + i, yIndex)).ToList(),
                    Symbol = symbol
                });
            }

            yIndex++;
        }

        var sum = schematicValues.Where(v => v.IsPartNumber).Sum(v => v.Value);

        return sum;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile);
        var regex = new Regex(@"\d+");
        var yIndex = 0;
        var matrix = new List<string>();
        var schematicValues = new List<SchematicValue>();

        foreach (var line in data)
            matrix.Add(line);

        var dimensions = matrix[0].Length;

        yIndex = 0;
        foreach (var line in matrix)
        {
            foreach (var match in regex.Matches(line).ToArray())
            {
                var value = int.Parse(match.Value);
                var adjacentPositions = GetAdjectPositions(new(match.Index, yIndex), match.Length, dimensions);
                ValuePosition<char> symbol = null!;

                foreach (var position in adjacentPositions)
                {
                    var c = matrix[position.Y][position.X];
                    if (c != '.' && !char.IsDigit(c))
                    {
                        symbol = new(c, position.X, position.Y);
                        break;
                    }
                }

                schematicValues.Add(new SchematicValue
                {
                    Value = value,
                    ValuePositions = Enumerable.Range(0, match.Length).Select(i => new ValuePosition<char>(matrix[yIndex][match.Index + i], match.Index + i, yIndex)).ToList(),
                    Symbol = symbol
                });
            }

            yIndex++;
        }

        var asteriskValues = schematicValues
            .Where(v => v.Symbol != null && v.Symbol.Value == '*')
            .ToList();

        var gearPairs = new List<SchematicValue>();
        var sum = 0;

        foreach (var value in asteriskValues)
        {
            if (gearPairs.Contains(value))
                continue;

            var gearPosition = new Position(value.Symbol.X, value.Symbol.Y);
            var adjecentValues = GetAdjectPositions(gearPosition, 1, dimensions);

            var gearPair = adjecentValues.SelectMany(pos => asteriskValues
                .Where(x => x.ValuePositions.Contains(pos)))
                .DistinctBy(x => x.ValuePositions.First())
                .ToList();

            if (gearPair.Count == 2)
            {
                sum += gearPair.Select(c => c.Value).Product();
                gearPairs.AddRange(gearPair);
            }
        }

        return sum;
    }
}
