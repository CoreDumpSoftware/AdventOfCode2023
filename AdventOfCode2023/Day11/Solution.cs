using System.Data;
using AdventOfCode2023.Day10;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day11;

public class Solution : SolutionBase
{
    private const char FillCharacter = 'x';

    protected override string SolutionInput { get; init; } = "11.txt";
    protected override string SampleInputOne { get; set; } = "11_sample_1.txt";

    public override long PartOne()
    {
        var result = CalculateGalaxyDistanceSums(SampleInputOne, 2);

        return result;
    }

    public override long PartTwo()
    {
        var result = CalculateGalaxyDistanceSums(SolutionInput, 1000000);

        return result;
    }

    private long CalculateGalaxyDistanceSums(string filename, long amplitude)
    {
        var matrix = new Matrix<char>(GetFileContents(filename).Select(l => l.ToCharArray()).ToArray());

        ModifyEmptyColumns(matrix);
        ModifyEmptyRows(matrix);

        MarkIntersections(matrix);

        matrix.PrintMatrix(true);

        var galaxyPositions = EnumerateCharacter(matrix, '#').ToList();

        var result = GetShortestPathBetweenPositions(matrix, galaxyPositions, amplitude).Sum();

        return result;
    }

    private static void ModifyEmptyRows(Matrix<char> matrix)
    {
        foreach (var yIndex in matrix.VerticalBounds)
        {
            if (matrix.GetRow(yIndex).All(v => v == '.' || v == FillCharacter))
            {
                matrix.SetRowValues(yIndex, FillCharacter);
            }
        }
    }

    private static void ModifyEmptyColumns(Matrix<char> matrix)
    {
        foreach (var xIndex in matrix.HorizontalBounds)
        {
            if (matrix.GetColumn(xIndex).All(v => v == '.' || v == FillCharacter))
            {
                matrix.SetColumnValues(xIndex, FillCharacter);
            }
        }
    }

    private static IEnumerable<Position> EnumerateCharacter(Matrix<char> matrix, char toFind)
    {
        foreach (var yIndex in matrix.VerticalBounds)
        {
            foreach (var xIndex in matrix.HorizontalBounds)
            {
                var position = new Position(xIndex, yIndex);
                if (matrix[position] == toFind)
                    yield return position;
            }
        }
    }

    private static void MarkIntersections(Matrix<char> matrix)
    {
        foreach (var yIndex in matrix.VerticalBounds)
        {
            var row = matrix.GetRow(yIndex);
            foreach (var xIndex in matrix.HorizontalBounds)
            {
                if (matrix.GetAdjacentValues(new(xIndex, yIndex), true).All(v => v == FillCharacter || v == char.ToUpper(FillCharacter)))
                    matrix[xIndex, yIndex] = 'X';
            }
        }
    }

    private static IEnumerable<long> GetShortestPathBetweenPositions(
        Matrix<char> matrix,
        List<Position> positions,
        long amplitude)
    {
        for (var a = 0; a < positions.Count; a++)
        {
            var src = positions[a];

            for (var b = a + 1; b < positions.Count; b++)
            {
                var dst = positions[b];
                var currentPos = src;
                var direction = currentPos.GetDirectionTo(dst);
                long distance = 0;

                while (direction != Direction.Same)
                {
                    if ((direction & Direction.North) > 0)
                        currentPos = new(currentPos.X, currentPos.Y - 1);
                    else if ((direction & Direction.South) > 0)
                        currentPos = new(currentPos.X, currentPos.Y + 1);
                    else if ((direction & Direction.West) > 0)
                        currentPos = new(currentPos.X - 1, currentPos.Y);
                    else if ((direction & Direction.East) > 0)
                        currentPos = new(currentPos.X + 1, currentPos.Y);

                    distance += matrix[currentPos] switch
                    {
                        'X' => amplitude * 2,
                        'x' => amplitude,
                        _ => 1
                    };

                    direction = currentPos.GetDirectionTo(dst);
                }

                yield return distance;
            }
        }
    }
}
