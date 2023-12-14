using System.Numerics;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day10;

public class Solution : SolutionBase
{
    private const int MatrixDimensions = 140;
    private static readonly Range<int> MatrixRange = new(0, MatrixDimensions - 1);
    private static readonly ValuePosition<char> Start = new('S', 25, 42);

    protected override string SolutionInput { get; init; } = "10.txt"; // S @ (25, 42)

    public override long PartOne()
    {
        var matrix = GetMatrix(GetFileContents(SolutionInput));
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
            pathOneMarker = GetNextSection(matrix, pathOneMarker, pathOnePrevious, false);
            pathOnePrevious = temp;

            temp = pathTwoMarker;
            pathTwoMarker = GetNextSection(matrix, pathTwoMarker, pathTwoPrevious, false);
            pathTwoPrevious = temp;
        }

        return steps;
    }

    public override long PartTwo()
    {
        var file = SolutionInput;

        var matrix = GetMatrix(GetFileContents(file));
        var width = matrix.HorizontalBounds.End;
        var height = matrix.VerticalBounds.End;
        var start = matrix.IndexOf('S');

        var startConnectingPieces = matrix.GetAdjacentValues(start, true)
            .Where(v => IsPipePiece(v.Value) && IsValidNextPipePiece(start, v))
            .ToList();

        var pathMarker = startConnectingPieces[0];
        var pathPrevious = start;

        var traceMatrixSrc = new char[height * 2][];
        foreach (var row in Enumerable.Range(0, height * 2))
            traceMatrixSrc[row] = Enumerable.Repeat(' ', width * 2).ToArray();

        var traceMatrix = new Matrix<char>(traceMatrixSrc);

        Console.WriteLine(string.Join("\r\n", traceMatrix.GetPrintableLines()));

        var direction = start.GetDirectionTo(pathMarker);
        Position traceMarker = new Position(start.X * 2, start.Y * 2);

        traceMatrix[traceMarker] = start.Value;
        traceMarker = traceMarker.ExtendByDirection(direction);

        traceMatrix[traceMarker] = direction.GetExtensionPipePiece();
        traceMarker = traceMarker.ExtendByDirection(direction);

        var value = matrix[pathMarker];
        direction = pathPrevious.GetDirectionTo(pathMarker);

        // If there's an even amount of pipes in the loop then the markers will pass each
        // other unless the previous value is checked.
        while (pathMarker.Value != 'S')
        {

            var temp = pathMarker;
            pathMarker = GetNextSection(matrix, pathMarker, pathPrevious, true);
            pathPrevious = temp;

            direction = pathPrevious.GetDirectionTo(pathMarker);

            traceMatrix[traceMarker] = value;
            traceMarker = traceMarker.ExtendByDirection(direction);

            traceMatrix[traceMarker] = direction.GetExtensionPipePiece();
            traceMarker = traceMarker.ExtendByDirection(direction);

            value = matrix[pathMarker];
        }

        // Add a border of empty space around the entire source matrix so we can flood fill easily
        var fillMatrixSource = traceMatrix.ToArray().Select(r => r.ToList()).ToList();
        fillMatrixSource.Insert(0, Enumerable.Repeat(' ', width * 2).ToList());
        fillMatrixSource.Add(Enumerable.Repeat(' ', width * 2).ToList());
        foreach (var row in fillMatrixSource)
        {
            row.Insert(0, ' ');
            row.Add(' ');
        }

        var fillMatrix = new Matrix<char>(fillMatrixSource.Select(r => r.ToArray()).ToArray());

        FloodFill(fillMatrix, new Position(0, 0));

        Console.WriteLine(string.Join("\r\n", fillMatrix.GetPrintableLines()));

        var fillMatrixArray = fillMatrix.ToArray();
        var resultArray = new char[(fillMatrix.VerticalBounds.Length - 2) / 2][];
        for (var y = 0; y < resultArray.Length; y++)
        {
            var sourceArray = fillMatrixArray[(y * 2) + 1];
            resultArray[y] = sourceArray.Skip(1).Where((x, i) => i % 2 == 0).ToArray();
        }

        var resultMatrix = new Matrix<char>(resultArray);
        Console.WriteLine(string.Join("\r\n", resultMatrix.GetPrintableLines()));

        var spacesWithinLoop = resultArray.SelectMany(r => r).Where(v => v == ' ').Count();

        return spacesWithinLoop;
    }

    private void FloodFill(Matrix<char> matrix, Position start)
    {
        HashSet<Position> visitedPositions = new();

        var stack = new Stack<Position>();

        stack.Push(start);

        while (stack.Count > 0)
        {
            var pos = stack.Pop();
            visitedPositions.Add(pos);

            if (IsPipePiece(matrix[pos], true) || matrix[pos] == '.')
                continue;

            matrix[pos] = '.';

            var adjacents = matrix.GetAdjacentValues(pos, false).Where(p => !visitedPositions.Contains(p) && !stack.Contains(p));
            foreach (var nextPos in adjacents)
                stack.Push(nextPos);
        }
    }

    private void FillOutside(Matrix<char> matrix, Position position, HashSet<Position> visitedPositions, int recursionDepth)
    {
        if (recursionDepth > 1000)
            return;

        visitedPositions.Add(position);

        var adjacentPositions = matrix.GetAdjacentValues(position, false).ToList();
        var filtered =adjacentPositions.Where(p => !visitedPositions.Contains(p)).ToList();

        foreach (var adjacentPosition in filtered)
        //foreach (var adjacentPosition in matrix.GetAdjacentValues(position, false).Where(p => !visitedPositions.Contains(p)))
        {
            var value = matrix[adjacentPosition];
            if (IsPipePiece(value) || value == '.' || value == 'S')
                continue;
            else
            {
                matrix[adjacentPosition] = '.';
                FillOutside(matrix, adjacentPosition, visitedPositions, recursionDepth + 1);
            }
        }
    }

    private ValuePosition<char> GetNextSection(Matrix<char> matrix, ValuePosition<char> current, Position? previous, bool includeStartAsPipePiece) => matrix
        .GetAdjacentValues(current, true)
        .Where(v => IsPipePiece(v.Value, includeStartAsPipePiece)
                 && (previous != null && v != previous)
                 && IsValidNextPipePiece(current, v))
        .Single(v => v != current); // explicit use of single here to catch bugs in navigation

    private Matrix<char> GetMatrix(IEnumerable<string> lines) =>
        new Matrix<char>(lines.Select(l => l.ToCharArray()).ToArray());

    private bool IsPipePiece(char c, bool includeStart = false) => c switch
    {
        '|' => true,
        '-' => true,
        'L' => true,
        'J' => true,
        '7' => true,
        'F' => true,
        'S' => includeStart,
        _ => false
    };

    private bool IsValidNextPipePiece(ValuePosition<char> src, ValuePosition<char> dst)
    {
        var direction = src.GetDirectionTo(dst);

        return direction switch
        {
            Direction.North => src.Value switch
            {
                'S' or '|' or 'J' or 'L' => dst.Value.IsOneOf('|', '7', 'F', 'S'),
                _ => false
            },
            Direction.South => src.Value switch
            {
                'S' or '|' or '7' or 'F' => dst.Value.IsOneOf('|', 'J', 'L', 'S'),
                _ => false
            },
            Direction.West => src.Value switch
            {
                'S' or '-' or '7' or 'J' => dst.Value.IsOneOf('-', 'L', 'F', 'S'),
                _ => false
            },
            Direction.East => src.Value switch
            {
                'S' or '-' or 'L' or 'F' => dst.Value.IsOneOf('-', 'J', '7', 'S'),
                _ => false
            },
            _ => false
        };
    }
}
