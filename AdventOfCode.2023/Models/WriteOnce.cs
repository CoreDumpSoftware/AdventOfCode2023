using AdventOfCode.Y2023.Exceptions;

namespace AdventOfCode.Y2023.Models;

public struct WriteOnce<T>
{
    public static implicit operator T(WriteOnce<T> input) => input.Value;
    public static implicit operator WriteOnce<T>(T input) => new() { Value = input };

    private T _value;

    public bool HasValue { get; private set; }
    public T Value
    {
        get => _value;
        set
        {
            if (HasValue)
                throw new WriteOnceException($"Value has already been set.");

            _value = value;
            HasValue = true;
        }
    }
}
