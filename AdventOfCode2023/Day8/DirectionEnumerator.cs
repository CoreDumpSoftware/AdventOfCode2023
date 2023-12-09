using System.Collections;

namespace AdventOfCode2023.Day8;

public class DirectionEnumerator : IEnumerator<int>
{
    private readonly string _directionsLine;
    private int _index = 0;
    private readonly int _length;

    public DirectionEnumerator(string directionsLine)
    {
        _directionsLine = directionsLine;
        _length = _directionsLine.Length;
    }

    public int Current
    {
        get => _directionsLine[_index] == 'L' ? -1 : 1;
    }

    object IEnumerator.Current
    {
        get => _directionsLine[_index] == 'L' ? -1 : 1;
    }

    public void Dispose()
    {
    }

    public int GetNextDirection()
    {
        var x = _directionsLine[_index++];

        if (_index >= _length)
            _index = 0;

        return _directionsLine[_index++] == 'L'
            ? -1
            : 1;
    }

    public bool MoveNext()
    {
        if (_index >= _length - 1)
            Reset();
        else
            _index++;

        return true;
    }

    public void Reset()
    {
        _index = 0;
    }
}