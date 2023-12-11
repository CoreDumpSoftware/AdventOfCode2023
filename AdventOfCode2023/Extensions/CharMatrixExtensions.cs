using System.Text;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Extensions;

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
}

public static class ArrayExtensions
{
    public static T[] SubArray<T>(this T[] data, int index, int length)
    {
        var result = new T[length];
        Array.Copy(data, index, result, 0, length);

        return result;
    }
}