namespace AdventOfCode2023.Models;

/// <summary>
/// Provides a layer over the out-of-box dictionary that allows searching by index
/// without throwing an exception if not found.
/// </summary>
/// <typeparam name="TKey">Key type of the dictionary.</typeparam>
/// <typeparam name="TValue">Value type stored by the key.</typeparam>
public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull
{
    public DefaultDictionary() { }

    public DefaultDictionary(IDictionary<TKey, TValue> dict) : base(dict) { }

    public TValue DefaultValue { get; set; } = default!;

    public DefaultDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs) : base(pairs) { }

    public new TValue this[TKey key]
    {
        get => TryGetValue(key, out var t) ? t : DefaultValue;
        set => base[key] = value;
    }
}