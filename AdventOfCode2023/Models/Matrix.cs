namespace AdventOfCode2023.Models;

public class Matrix<T>
{
    private readonly T[][] _matrix;
    public Range<int> VerticalBounds { get; init; }
    public Range<int> HorizontalBounds { get; init; }

    public T this[int x, int y]
    {
        get
        {
            CheckBounds(x, y);

            return _matrix[y][x];
        }
        set
        {
            CheckBounds(x, y);

            _matrix[y][x] = value;
        }
    }

    public T this[Position p]
    {
        get => this[p.X, p.Y];
        set => this[p.X, p.Y] = value;
    }

    public Matrix(T[][] input)
    {
        _matrix = input;
        HorizontalBounds = new Range<int>(0, input[0].Length - 1);
        VerticalBounds = new Range<int>(0, input.Length - 1);
    }

    public Matrix(int dimensions) : this(dimensions, dimensions) { }

    public Matrix(int width, int height)
    {
        HorizontalBounds = new Range<int>(0, width);
        VerticalBounds = new Range<int>(0, height);

        _matrix = VerticalBounds.Select(r => new T[HorizontalBounds.Length]).ToArray();
    }

    public IEnumerable<ValuePosition<T>> GetAdjacentValues(Position p, bool skipCorners) => GetAdjacentValues(p.X, p.Y, skipCorners);

    public IEnumerable<ValuePosition<T>> GetAdjacentValues(int x, int y, bool skipCorners)
    {
        CheckBounds(x, y);

        var range = new Range<int>(-1, 1);

        if (skipCorners)
        {
            if (CheckVerticalBounds(y - 1))
                yield return GetValuePosition(x, y - 1);
            if (CheckHorizontalBounds(x - 1))
                yield return GetValuePosition(x - 1, y);
            if (CheckHorizontalBounds(x + 1))
                yield return GetValuePosition(x + 1, y);
            if (CheckVerticalBounds(y + 1))
                yield return GetValuePosition(x, y + 1);
        }
        else
        {
            foreach (var yIndex in range.Select(v => v + y))
            {
                if (!CheckVerticalBounds(yIndex))
                    continue;

                foreach (var xIndex in range.Select(v => v + x))
                {
                    if (yIndex == x && xIndex == y)
                        continue;

                    if (CheckHorizontalBounds(xIndex))
                        yield return GetValuePosition(xIndex, yIndex);
                }
            }
        }
    }

    public IEnumerable<T> GetRow(int row) =>
        HorizontalBounds.Select(x => this[x, row]);

    public IEnumerable<T> GetColumn(int column) =>
        VerticalBounds.Select(y => this[column, y]);

    public T[][] ToArray() =>
        _matrix.Select(r => r.ToArray()).ToArray();

    private void CheckBounds(int x, int y)
    {
        var exceptions = new List<Exception>();

        if (!CheckHorizontalBounds(x))
            exceptions.Add(GetOutOfBoundsException(x, GetHorizontalBoundsError));

        if (!CheckVerticalBounds(y))
            exceptions.Add(GetOutOfBoundsException(y, GetVerticalBoundsError));

        if (exceptions.Any())
            throw new AggregateException(exceptions.ToArray());
    }

    private bool CheckHorizontalBounds(int xPos) =>
        HorizontalBounds.Contains(xPos);

    private bool CheckVerticalBounds(int yPos) =>
        VerticalBounds.Contains(yPos);

    private Exception GetOutOfBoundsException(int value, Func<int, string> getErrorMessage)
    {
        return new ArgumentOutOfRangeException(getErrorMessage(value));
    }

    private string GetHorizontalBoundsError(int xPos) =>
        $"{xPos} is out of horizontal bounds. Value must satisfy: {HorizontalBounds}";

    private string GetVerticalBoundsError(int yPos) =>
        $"{yPos} is out of vertical bounds. Value must satisfy: {VerticalBounds}";

    private ValuePosition<T> GetValuePosition(int x, int y) => new ValuePosition<T>(_matrix[y][x], x, y);
}
