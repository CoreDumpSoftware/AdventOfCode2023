namespace AdventOfCode2023.Extensions;

public static class IntCollectionExtensions
{
    public static int Product(this IEnumerable<int> values)
    {
        var result = values.First();
        foreach (var value in values.Skip(1))
        {
            result *= value;
        }

        return result;
    }
}
