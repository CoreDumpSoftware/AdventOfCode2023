using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day10;

public class Solution : SolutionBase
{
    private const int MatrixDimensions = 140;
    private static readonly Range<int> MatrixRange = new(0, MatrixDimensions - 1);
    private static readonly ValuePosition<char> Start = new('S', 25, 42);

    protected override string PartOneInputFile { get; init; } = "10.txt"; // S @ (25, 42)
    protected override string PartTwoInputFile { get; init; } = "10_sample1.txt";

    public override long PartOne()
    {
        var matrix = GetMatrix(GetFileContents(PartOneInputFile));
        var visitedPositions = new List<Position>();

        var startConnectingPieces = matrix.GetAdjacentValues(Start, true)
            .Where(v => IsPipePiece(v.Value) && IsValidNextPipePiece(Start, v))
            .ToList();

        var pathOneMarker = startConnectingPieces[0];
        var pathOnePrevious = Start;

        var pathTwoMarker = startConnectingPieces[1];
        var pathTwoPrevious = Start;

        var steps = 1; // Start at 1 since we navigated from start to the two markers

        // If there's an even amount of pipes in the loop then the markers will pass each
        // other unless the previous value is checked.
        while(pathOneMarker != pathTwoMarker && pathOnePrevious != pathTwoMarker)
        {
            steps++;

            var temp = pathOneMarker;
            pathOneMarker = GetNextSection(matrix, pathOneMarker, pathOnePrevious);
            pathOnePrevious = temp;

            temp = pathTwoMarker;
            pathTwoMarker = GetNextSection(matrix, pathTwoMarker, pathTwoPrevious);
            pathTwoPrevious = temp;
        }

        return steps;
    }

    public override long PartTwo()
    {

        var matrix = GetMatrix(GetFileContents(PartOneInputFile));
        var visitedPositions = new List<Position>();

        var startConnectingPieces = matrix.GetAdjacentValues(Start, true)
            .Where(v => IsPipePiece(v.Value) && IsValidNextPipePiece(Start, v))
            .ToList();

        var pathOneMarker = startConnectingPieces[0];
        var pathOnePrevious = Start;

        var pathTwoMarker = startConnectingPieces[1];
        var pathTwoPrevious = Start;

        var steps = 1; // Start at 1 since we navigated from start to the two markers
        var traceMatrixSrc = new char[MatrixDimensions][];
        foreach (var row in MatrixRange)
            traceMatrixSrc[row] = Enumerable.Repeat(' ', MatrixDimensions).ToArray();

        var traceMatrix = new Matrix<char>(traceMatrixSrc);
        traceMatrix[Start] = Start.Value;
        traceMatrix[pathOneMarker] = pathOneMarker.Value;
        traceMatrix[pathTwoMarker] = pathTwoMarker.Value;

        // If there's an even amount of pipes in the loop then the markers will pass each
        // other unless the previous value is checked.
        while (pathOneMarker != pathTwoMarker && pathOnePrevious != pathTwoMarker)
        {
            var temp = pathOneMarker;
            pathOneMarker = GetNextSection(matrix, pathOneMarker, pathOnePrevious);
            pathOnePrevious = temp;

            temp = pathTwoMarker;
            pathTwoMarker = GetNextSection(matrix, pathTwoMarker, pathTwoPrevious);
            pathTwoPrevious = temp;

            traceMatrix[pathOneMarker] = pathOneMarker.Value;
            traceMatrix[pathTwoMarker] = pathTwoMarker.Value;
        }

        //Console.WriteLine(string.Join("\r\n", matrix.GetPrintableLines()));
        //File.WriteAllLines("loop.txt", traceMatrix.GetPrintableLines());

        // Add a border of empty space around the entire source matrix so we can flood fill easily
        var fillMatrixSource = traceMatrix.ToArray().Select(r => r.ToList()).ToList();
        fillMatrixSource.Insert(0, Enumerable.Repeat(' ', MatrixDimensions).ToList());
        fillMatrixSource.Add(Enumerable.Repeat(' ', MatrixDimensions).ToList());
        foreach (var row in fillMatrixSource)
        {
            row.Insert(0, ' ');
            row.Add(' ');
        }


        var fillMatrix = new Matrix<char>(fillMatrixSource.Select(r => r.ToArray()).ToArray());
        fillMatrix[0, 0] = '.';

        FillOutside(fillMatrix, new Position(0, 0));
        Console.WriteLine(string.Join("\r\n", fillMatrix.GetPrintableLines()));

        var spacesWithinLoop = fillMatrix.ToArray().SelectMany(r => r).Where(v => v == ' ').Count();

        return spacesWithinLoop;
    }

    private void FillOutside(Matrix<char> matrix, Position position)
    {
        foreach (var adjacentPosition in matrix.GetAdjacentValues(position, false))
        {
            var value = matrix[adjacentPosition];
            if (IsPipePiece(value) || value == '.')
                continue;
            else
            {
                matrix[adjacentPosition] = '.';
                FillOutside(matrix, adjacentPosition);
            }
        }
    }

    private ValuePosition<char> GetNextSection(Matrix<char> matrix, ValuePosition<char> current, Position? previous)
    {
        var s1 = matrix.GetAdjacentValues(current, true).ToArray();
        var s2 = s1.Where(v => IsPipePiece(v.Value)).ToArray();
        var s3 = s2.Where(v => v != null & v != previous);
        var s4 = s3.Where(v => IsValidNextPipePiece(current, v));
        var result = s4.Single();

        return result;
        //.Where(v => IsPipePiece(v.Value)
        //         && (previous != null && v != previous)
        //         && IsValidNextPipePiece(current, v))
        //.Single(v => v != current); // explicit use of single here to catch bugs in navigation
    }

    //private ValuePosition<char> GetNextSection(Matrix<char> matrix, ValuePosition<char> current, Position? previous) => matrix
    //    .GetAdjacentValues(current, true)
    //    .Where(v => IsPipePiece(v.Value)
    //             && (previous != null && v != previous)
    //             && IsValidNextPipePiece(current, v))
    //    .Single(v => v != current); // explicit use of single here to catch bugs in navigation

    private Matrix<char> GetMatrix(IEnumerable<string> lines) =>
        new Matrix<char>(lines.Select(l => l.ToCharArray()).ToArray());

    private bool IsPipePiece(char c) => c switch
    {
        '|' => true,
        '-' => true,
        'L' => true,
        'J' => true,
        '7' => true,
        'F' => true,
        _ => false
    };

    private bool IsValidNextPipePiece(ValuePosition<char> src, ValuePosition<char> dst)
    {
        var direction = src.GetDirectionTo(dst);

        return direction switch
        {
            Direction.North => src.Value switch
            {
                'S' or '|' or 'J' or 'L' => dst.Value.IsOneOf('|', '7', 'F'),
                _ => false
            },
            Direction.South => src.Value switch
            {
                'S' or '|' or '7' or 'F' => dst.Value.IsOneOf('|', 'J', 'L'),
                _ => false
            },
            Direction.West => src.Value switch
            {
                'S' or '-' or '7' or 'J' => dst.Value.IsOneOf('-', 'L', 'F'),
                _ => false
            },
            Direction.East => src.Value switch
            {
                'S' or '-' or 'L' or 'F' => dst.Value.IsOneOf('-', 'J', '7'),
                _ => false
            },
            _ => false
        };
    }
}
