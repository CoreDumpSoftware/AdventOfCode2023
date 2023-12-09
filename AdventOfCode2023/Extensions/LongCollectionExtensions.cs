namespace AdventOfCode2023.Extensions;

public static class LongCollectionExtensions
{
    public static long Product(this IEnumerable<long> values)
    {
        var result = values.First();
        foreach (var value in values.Skip(1))
        {
            result *= value;
        }

        return result;
    }

    public static long LeastCommonMultiple(this IEnumerable<long> values)
    {
        if (values.Count() == 2)
        {
            return LeastCommonMultiple(values.First(), values.Last());
        }
        else
        {
            return LeastCommonMultiple(values.First(), LeastCommonMultiple(values.Skip(1)));
        }
    }

    private static long LeastCommonMultiple(long a, long b)
    {
        return (a * b) / GreaterCommonDenominator(a, b);
    }

    private static long GreaterCommonDenominator(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}