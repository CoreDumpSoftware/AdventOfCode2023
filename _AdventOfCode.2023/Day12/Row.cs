using System.Collections;

namespace AdventOfCode2023.Day12;

public class Row : IEnumerable<char>
{
    private readonly char[] _row;
    private readonly Dictionary<char, int> _charCounts;

    public int OperationalCount => _charCounts.TryGetValue('.', out var count) ? count : 0;
    public int DamagedCount => _charCounts.TryGetValue('#', out var count) ? count : 0;
    public int UnknownCount => _charCounts.TryGetValue('?', out var count) ? count : 0;

    public Row(char[] row)
    {
        _row = row;
        _charCounts = _row.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
    }

    private Row(char[] row, Dictionary<char, int> charCounts)
    {
        _row = row.ToArray();
        _charCounts = charCounts.ToDictionary();
    }

    public char this[int index]
    {
        get
        {
            if (index < 0 || index >= _row.Length)
                throw new ArgumentOutOfRangeException("index");

            return _row[index];
        }
        set
        {
            if (index < 0 || index >= _row.Length)
                throw new ArgumentOutOfRangeException("index");

            if (value != '.' && value != '#' && value != '?')
                throw new ArgumentException("Invalid char to insert into row.");

            var prev = _row[index];
            if (prev == value)
                return;

            if (_charCounts.TryGetValue(prev, out var count))
                _charCounts[prev]--;
            else
                _charCounts.Add(prev, 0);

            if (_charCounts.TryGetValue(value, out count))
                _charCounts[value]++;
            else
                _charCounts.Add(value, 1);

            _row[index] = value;
        }
    }

    public Row Clone() => new(_row, _charCounts);

    public IEnumerator<char> GetEnumerator() => _row.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public override string ToString() => new string(_row);
}