namespace AdventOfCode2023.Utilties;

public struct WriteOnce<T>
{
    public static implicit operator T(WriteOnce<T> input) => input.Value;
    public static implicit operator WriteOnce<T>(T input) => new WriteOnce<T> { Value = input };

    private T _value;

    public bool HasValue { get; private set;  }
    public T Value
    {
        get => _value;
        set
        {
            if (HasValue)
                throw new InitOnlyException($"Value has already been set.");

            _value = value;
            HasValue = true;
        }
    }
}
