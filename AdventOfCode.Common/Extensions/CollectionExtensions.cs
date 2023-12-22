namespace AdventOfCode.Common.Extensions;

public static class CollectionExtensions
{
    public static bool SafeAny<T>(this IEnumerable<T> input) => input != null && input.Any();
}
