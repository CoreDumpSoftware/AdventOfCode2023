using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day5;

public class MapRow
{
    public Range Source { get; init; }
    public Range Destination { get; init; }
    public long Delta { get; init; }
    public long RangeLength { get; init; }

    private MapRow(long destination, long source, long length)
    {
        Source = new(source, length);
        Destination = new(destination, length);
        Delta = destination - source;
        RangeLength = length;
    }

    public long GetDestinationBySource(long source) => Source.Contains(source)
        ? source + Delta
        : throw new ArgumentOutOfRangeException(nameof(source));

    public static MapRow Parse(string line)
    {
        var values = line.ParseLongs().ToArray();

        var dst = values[0];
        var src = values[1];
        var len = values[2];

        return new MapRow(dst, src, len);
    }
}
