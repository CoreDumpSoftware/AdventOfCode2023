using System.Collections;

namespace AdventOfCode.Y2023.Day12;

public class RowPermutations : IEnumerable<string>
{
    private readonly string _row;
    private readonly int[] _groups;
    private readonly int _expectedNumDamaged;

    public RowPermutations(string row, IEnumerable<int> groups)
    {
        _row = row;
        _groups = groups.ToArray();
        _expectedNumDamaged = _groups.Sum();
    }

    public IEnumerable<string> ScanRow(Row row)
    {
        if ((row.DamagedCount > _expectedNumDamaged) ||
            (row.DamagedCount + row.UnknownCount < _expectedNumDamaged))
            yield break;

        var index = row.IndexOf('?');
        if (index == -1) // end of line
        {
            if (row.DamagedCount != _expectedNumDamaged)
                yield break;
            if (row.DamagedCount == _expectedNumDamaged)
            {
                if (FindGroupPattern(row))
                    yield return row.ToString();

                yield break;
            }
        }

        var copy = row.Clone();

        copy[index] = '.';
        foreach (var permutation in ScanRow(copy))
            yield return permutation;

        copy[index] = '#';
        foreach (var permutation in ScanRow(copy))
            yield return permutation;
    }

    private int CountTotalPossibleDamaged(IEnumerable<char> window) =>
        window.Count(c => c == '#' || c == '?');

    private bool FindGroupPattern(Row row)
    {
        var foundGroups = row.ToString().Split('.').Select(p => p.Count()).Where(c => c > 0).ToArray();
        return foundGroups.SequenceEqual(_groups);
    }

    public IEnumerator<string> GetEnumerator()
    {
        var row = new Row(_row.ToCharArray());
        return ScanRow(row).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        this.GetEnumerator();
}

public static class CharEnumerableExtensions
{
    public static int IndexOf(this IEnumerable<char> chars, char toFind)
    {
        var index = 0;

        foreach (var c in chars)
        {
            if (c == toFind)
                return index;

            index++;
        }

        return -1;
    }
}
