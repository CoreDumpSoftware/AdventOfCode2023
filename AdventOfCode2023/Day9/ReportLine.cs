using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day9;

public class ReportLine
{
    public List<long> Values { get; set; }
    public List<List<long>> Decompositions { get; set; } = new();

    public ReportLine(string line)
    {
        Values = line.Split(' ').Select(long.Parse).ToList();
        Decompositions.Add(Decompose(Values));

        while (Decompositions.Last().Any(v => v != 0))
        {
            Decompositions.Add(Decompose(Decompositions.Last()));
        }
    }

    public static List<long> Decompose(List<long> values)
    {
        var result = new List<long>();

        for (var i = 0; i < values.Count() - 1; i++)
            result.Add(values[i + 1] - values[i]);

        return result;
    }

    public long ExtrapolateForward()
    {
        long previousDelta = 0;
        foreach (var decompositon in Decompositions.AsEnumerable().Reverse().Skip(1))
        {
            previousDelta = decompositon.Last() + previousDelta;
            decompositon.Add(previousDelta);
        }

        Values.Add(Values.Last() + previousDelta);

        return Values[Values.Count - 1];
    }

    public long ExtrapolateBackward()
    {
        long previousDelta = 0;
        foreach (var decompositon in Decompositions.AsEnumerable().Reverse().Skip(1))
        {
            previousDelta = decompositon[0] - previousDelta;
            decompositon.Insert(0, previousDelta);
        }

        Values.Insert(0, Values[0] - previousDelta);

        return Values[0];
    }
}
