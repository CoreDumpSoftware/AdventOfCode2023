using System.Collections;
using System.Numerics;

namespace AdventOfCode2023.Models;

/// <summary>
/// Represent a range of numbers inclusive of the start and end values.
/// For example, a start of 0 and end of 100 results in a range of 0, 1, 2, ..., 98, 99, 100.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="start"></param>
/// <param name="end"></param>
public class Range<T>(T start, T end) : IEnumerable<int>
    where T : INumber<T>
{
    public T Start { get; set; } = start;
    public T End { get; set; } = end;
    public long Length { get; set; } = long.CreateChecked(end) - long.CreateChecked(start) + 1;

    public bool Contains(T value) =>
        value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;

    public IEnumerator<int> GetEnumerator() => GetValues().GetEnumerator();

    /// <summary>
    /// Warning: do not use for values exceeding Int32 Min/Max values!
    /// </summary>
    /// <returns>Range of numbers from <see cref="Start"/> until <see cref="End"/>.</returns>
    private IEnumerable<int> GetValues() => Enumerable.Range(
        int.CreateChecked(Start),
        int.CreateChecked(Length)
    );

    public override string ToString() =>
        $"{start} >= n <= {end}";

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}