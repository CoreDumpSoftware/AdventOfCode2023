namespace AdventOfCode.Y2023.Extensions;

public static class StringExtensions
{
    public static IEnumerable<int> ParseInts(this string input, char delimiter = ' ') =>
        input.Trim().Split(delimiter).Select(int.Parse);

    public static IEnumerable<long> ParseLongs(this string input, char delimiter = ' ') =>
        input.Trim().Split(delimiter).Select(long.Parse);

    public static string ReplaceAt(this string str, int index, int length, string replace)
    {
        return str.Remove(index, Math.Min(length, str.Length - index))
                .Insert(index, replace);
    }
}
