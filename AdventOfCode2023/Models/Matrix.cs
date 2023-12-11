namespace AdventOfCode2023.Models;

public class Matrix<T>
    where T: new()
{
    protected readonly List<List<T>> _matrix;
    public Range<int> VerticalBounds { get; protected set; }
    public Range<int> HorizontalBounds { get; protected set; }

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
        _matrix = input.Select(r => r.ToList()).ToList();
        HorizontalBounds = new Range<int>(0, input[0].Length);
        VerticalBounds = new Range<int>(0, input.Length);
    }

    public Matrix(int dimensions) : this(dimensions, dimensions) { }

    public Matrix(int width, int height)
    {
        HorizontalBounds = new Range<int>(0, width);
        VerticalBounds = new Range<int>(0, height);

        _matrix = VerticalBounds.Select(r => new List<T>((int)HorizontalBounds.Length)).ToList();
    }

    public void InsertRow(int index, Func<Position, T> setValueFunction = null!)
    {
        VerticalBounds = new Range<int>(0, VerticalBounds.End + 1);

        if (setValueFunction != null)
            _matrix.Insert(index, HorizontalBounds.Select(x => setValueFunction(new Position(x, index))).ToList());
        else
            _matrix.Insert(index, new List<T>((int)HorizontalBounds.Length));
    }

    public void InsertColumn(int index, Func<Position, T> setValueFunction = null!)
    {
        HorizontalBounds = new Range<int>(0, HorizontalBounds.End + 1);

        foreach (var yIndex in VerticalBounds)
        {
            var row = _matrix[yIndex];

            if (setValueFunction != null)
                row.Insert(index, setValueFunction(new Position(index, yIndex)));
            else
                row.Insert(index, default!);
        }
    }

    public void SetRowValues(int yIndex, T value)
    {
        foreach (var xIndex in HorizontalBounds)
        {
            this[xIndex, yIndex] = value;
        }
    }

    public void SetColumnValues(int xIndex, T value)
    {
        foreach (var yIndex in VerticalBounds)
        {
            this[xIndex, yIndex] = value;
        }
    }

    public IEnumerable<ValuePosition<T>> GetAdjacentValues(Position p, bool skipCorners) => GetAdjacentValues(p.X, p.Y, skipCorners);

    public IEnumerable<ValuePosition<T>> GetAdjacentValues(int x, int y, bool skipCorners)
    {
        CheckBounds(x, y);

        var range = new Range<int>(-1, 2);

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
                    if (yIndex == y && xIndex == x)
                        continue;

                    if (CheckHorizontalBounds(xIndex))
                        yield return GetValuePosition(xIndex, yIndex);
                }
            }
        }
    }

    public IEnumerable<IEnumerable<T>> Rows =>
        _matrix.AsEnumerable().Select(row => row.AsEnumerable());

    public IEnumerable<IEnumerable<T>> Columns =>
        HorizontalBounds.Select(x => VerticalBounds.Select(y => this[x, y]));

    public IEnumerable<T> GetRow(int row) =>
        HorizontalBounds.Select(x => this[x, row]);

    public IEnumerable<T> GetColumn(int column) =>
        VerticalBounds.Select(y => this[column, y]);

    public T[][] ToArray() =>
        _matrix.Select(r => r.ToArray()).ToArray();

    protected void CheckBounds(int x, int y)
    {
        var exceptions = new List<Exception>();

        if (!CheckHorizontalBounds(x))
            exceptions.Add(GetOutOfBoundsException(x, GetHorizontalBoundsError));

        if (!CheckVerticalBounds(y))
            exceptions.Add(GetOutOfBoundsException(y, GetVerticalBoundsError));

        if (exceptions.Any())
            throw new AggregateException(exceptions.ToArray());
    }

    protected bool CheckHorizontalBounds(int xPos) =>
        HorizontalBounds.Contains(xPos);

    protected bool CheckVerticalBounds(int yPos) =>
        VerticalBounds.Contains(yPos);

    protected Exception GetOutOfBoundsException(int value, Func<int, string> getErrorMessage)
    {
        return new ArgumentOutOfRangeException(getErrorMessage(value));
    }

    protected string GetHorizontalBoundsError(int xPos) =>
        $"{xPos} is out of horizontal bounds. Value must satisfy: {HorizontalBounds}";

    protected string GetVerticalBoundsError(int yPos) =>
        $"{yPos} is out of vertical bounds. Value must satisfy: {VerticalBounds}";

    protected ValuePosition<T> GetValuePosition(int x, int y) => new ValuePosition<T>(_matrix[y][x], x, y);
}
