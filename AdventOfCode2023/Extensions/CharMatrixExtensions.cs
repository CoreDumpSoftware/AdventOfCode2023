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
}