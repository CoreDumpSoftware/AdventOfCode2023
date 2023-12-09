using System.Text;

namespace AdventOfCode2023.Extensions;

public static class TextReaderExtensions
{
    public static Token ReadUntil(this TextReader reader, params char[] characters)
    {
        var builder = new StringBuilder();

        var c = reader.Read();

        while (c > 0 && !characters.Contains((char)c))
        {
            builder.Append((char)c);
            c = reader.Read();
        }

        return new Token(builder.ToString().Trim(), c == -1 ? '\0' : (char)c);
    }

    public static void Skip(this TextReader reader, int count)
    {
        for (var i = 0; i < count; i++)
        {
            reader.Read();
        }
    }
}
