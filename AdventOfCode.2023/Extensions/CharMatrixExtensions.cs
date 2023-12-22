using System.Text;
using AdventOfCode.Y2023.Models;

namespace AdventOfCode.Y2023.Extensions;

public static class CharMatrixExtensions
{
    public static IEnumerable<string> GetPrintableLines(this Matrix<char> matrix)
    {
        var length = matrix.VerticalBounds.Length >= matrix.HorizontalBounds.Length
            ? matrix.VerticalBounds.Length
            : matrix.HorizontalBounds.Length;

        var builder = new StringBuilder((int)length);

        foreach (var yIndex in matrix.VerticalBounds)
        {
            foreach (var c in matrix.GetRow(yIndex))
                builder.Append(char.IsControl(c) || char.IsWhiteSpace(c) ? ' ' : c);

            yield return builder.ToString();

            builder.Clear();
        }
    }

    public static void PrintMatrix(this Matrix<char> matrix, bool printIndices = false)
    {
        if (!printIndices)
        {
            Console.WriteLine(string.Join("\n", matrix.GetPrintableLines()));
        }
        else
        {
            var rows = Enumerable.Range(0, (int)matrix.VerticalBounds.Length).Select(v => $"{v, 3}");
            var colStrings = Enumerable.Range(0, (int)matrix.HorizontalBounds.Length).Select(v => $"{v, 3}");
            var colLines = Enumerable.Range(0, 3).Select(i => new string(colStrings.Select(s => s[i]).ToArray()));

            foreach (var line in colLines)
            {
                Console.WriteLine($"    {line}");
            }

            Console.WriteLine();

            Console.WriteLine(string.Join('\n', matrix.GetPrintableLines().Select((l, i) => $"{i,3} {l}")));
        }
    }

    public static ValuePosition<char>? IndexOf(this Matrix<char> matrix, char c)
    {
        foreach (var yIndex in matrix.VerticalBounds)
        {
            foreach (var xIndex in matrix.HorizontalBounds)
            {
                var value = matrix[xIndex, yIndex];
                if (value == c)
                    return new ValuePosition<char>(value, xIndex, yIndex);
            }
        }

        return null!;
    }

    public static IEnumerable<ValuePosition<char>> GetVerticalAdjacents(this Matrix<char> matrix, int x, int y)
    {
        // This is really lazy implementation
        return matrix.GetAdjacentValues(x, y, true).Where(p => p.X == x);
    }

    public static IEnumerable<ValuePosition<char>> GetHorizontalAdjacents(this Matrix<char> matrix, int x, int y)
    {
        // This is really lazy implementation
        return matrix.GetAdjacentValues(x, y, true).Where(p => p.Y == y);
    }
}
