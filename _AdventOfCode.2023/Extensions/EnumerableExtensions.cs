namespace AdventOfCode2023.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> SplitBy<T>(this IEnumerable<T> values, Predicate<T> splitOn)
    {
        var currentSet = new List<T>();
        foreach (var value in values)
        {
            if (splitOn(value))
            {
                yield return currentSet;
                currentSet.Clear();
            }
            else
            {
                currentSet.Add(value);
            }
        }

        if (currentSet.Count > 0)
            yield return currentSet;
    }
}
