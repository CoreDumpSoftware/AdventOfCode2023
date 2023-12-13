using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day13;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "13.txt";
    protected override string PartTwoInputFile { get; init; }
    protected override string SampleInputOne { get; set; } = "13_sample_1.txt";
    protected override string SampleInputTwo { get; set; } = "13_sample_2.txt";

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile, true).ToArray();
        var knownValues = new Dictionary<string, BinaryString>();
        var sum = 0;

        var index = 1;
        foreach ((var rows, var columns) in OrganizeFileToGroups(data).Select(g => GetRowsAndColumns(g, knownValues)))
        {
            var mirrorIndices = FindMirrorIndices(rows);
            var isHorizontal = mirrorIndices != null;

            mirrorIndices ??= FindMirrorIndices(columns);

            var value = mirrorIndices.Left + 1;
            if (isHorizontal)
                value *= 100;

            sum += value;
            index++;
        }

        return sum;
    }

    private static int WhateverForNow(BinaryString[] set, BinaryString left, BinaryString right)
    {
        var result = 0;

        // line match but maybe has outer pairs that do not
        if (left.Number == right.Number)
        {
            var pairGood = true;
            var offByOneFound = false;
            var lIndex = left.Index - 1;
            var rIndex = right.Index + 1;
            while (pairGood && lIndex >= 0 && rIndex < set.Length)
            {
                var iLeft = set[lIndex];
                var iRight = set[rIndex];
                var bitDifferences = iLeft.GetBitDifferences(iRight).ToArray();
                if (iLeft.Number == iRight.Number)
                {
                    lIndex--;
                    rIndex++;
                }
                else if (bitDifferences.Length == 1)
                {
                    lIndex--;
                    rIndex++;

                    if (offByOneFound)
                    {
                        pairGood = false;
                        break;
                    }

                    offByOneFound = true;
                }
                else
                {
                    pairGood = false;
                }
            }

            return pairGood ? left.Index + 1 : 0;
        }
        // pair that has a single bit difference between the two sets
        else if (left.GetBitDifferences(right).Count() == 1)
        {
            if (VerifyLine(set, left.Index - 1, right.Index + 1))
            {
                if (result != 0)
                    throw new Exception("Multiple matches found!");

                return left.Index + 1;
            }
        }

        return 0;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile, true).ToArray();
        var knownValues = new Dictionary<string, BinaryString>();
        var sum = 0;

        var index = 1;
        foreach ((var rows, var columns) in OrganizeFileToGroups(data).Select(g => GetRowsAndColumns(g, knownValues)))
        {
            int result = 0;
            foreach ((var left, var right) in GetPotentialOneOffPairs(rows).Select(p => (rows[p.Left], rows[p.Right])))
            {
                var nextResult = WhateverForNow(rows, left, right);
                if (nextResult == 0)
                    continue;
                if (nextResult != 0 && result != 0)
                    throw new Exception("Multiple matches found!");

                result = (nextResult * 100);
            }

            foreach ((var left, var right) in GetPotentialOneOffPairs(columns).Select(p => (columns[p.Left], columns[p.Right])))
            {
                var nextResult = WhateverForNow(columns, left, right);
                if (nextResult == 0)
                    continue;
                if (nextResult != 0 && result != 0)
                    throw new Exception("Multiple matches found!");

                result = nextResult;
            }

            if (result == 0)
                throw new Exception($"No matches found on line {index}");

            sum += result;

            index++;
        }

        return sum;
    }

    private static IEnumerable<MirrorIndices> GetPotentialOneOffPairs(BinaryString[] set)
    {
        foreach (var pair in Enumerable.Range(0, set.Length - 1).Select(i => new MirrorIndices(i, i + 1)))
        {
            if (set[pair.Left].Number == set[pair.Right].Number && !VerifyLine(set, pair.Left, pair.Right))
            {
                //Console.WriteLine($"\tH Match:{pair.Left}, {pair.Right}");
                yield return pair;
            }
        }

        foreach (var pair in GetPotentialMirrorIndices(set))
        {
            //Console.WriteLine($"\tH Diff: {pair.Left}, {pair.Right}");
            yield return pair;
        }
    }

    private static IEnumerable<MirrorIndices> GetPotentialMirrorIndices(BinaryString[] columns)
    {
        for (var i = 0; i < columns.Length - 1; i++)
        {
            // Find values where they differ
            if ((Math.Abs(columns[i].BitCount - columns[i + 1].BitCount) == 1) &&
                (columns[i].Number.GetBitDifferences(columns[i + 1].Number).Count() == 1))
            {
                yield return new(i, i + 1);
            }
        }
    }

    private static IEnumerable<string[]> OrganizeFileToGroups(IEnumerable<string> lines)
    {
        var currentSet = new List<string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                yield return currentSet.ToArray();
                currentSet.Clear();
            }
            else
            {
                currentSet.Add(line);
            }
        }

        if (currentSet.Count > 0)
            yield return currentSet.ToArray();
    }

    private static (BinaryString[] Rows, BinaryString[] Columns) GetRowsAndColumns(string[] lines, Dictionary<string, BinaryString> knownValues)
    {
        var rows = lines.Select((l, i) => ParseBinaryString(l, i, knownValues)).ToArray();

        var columns = Enumerable.Range(0, lines[0].Length)
            .Select(i => lines.Select(l => l[i]).ToArray())
            .Select(x => new string(x))
            .Select((l, i) => ParseBinaryString(l, i, knownValues))
            .ToArray();

        return (rows, columns);
    }

    private static BinaryString ParseBinaryString(string line, int index, Dictionary<string, BinaryString> knownValues)
    {
        line = line.PadLeft(32, '0');

        if (knownValues.TryGetValue(line, out var result))
            return new BinaryString(line, index, result.Number, result.BitCount);

        var number = Convert.ToInt32(line, 2);
        var bitCount = number.CountBits();

        result = new BinaryString(line, index, number, bitCount);

        knownValues.Add(line, result);

        return result;
    }

    private MirrorIndices FindMirrorIndices(BinaryString[] sets)
    {
        for (var i = 0; i < sets.Length - 1; i++)
        {
            if (VerifyLine(sets, i, i + 1))
                return new MirrorIndices(i, i + 1);
        }

        return null;
    }

    private static bool VerifyLine(BinaryString[] lines, int left, int right)
    {
        if (left < 0 || right >= lines.Length)
            return true;

        return (lines[left].Number == lines[right].Number) && VerifyLine(lines, left - 1, right + 1);
    }

    private record MirrorIndices(int Left, int Right);

    private class BinaryString(string line, int index, int number, int bitCount)
    {
        public string Line { get; init; } = line;
        public int Index { get; init; } = index;
        public int Number { get; init; } = number;
        public int BitCount { get; init; } = bitCount;

        public IEnumerable<int> GetBitDifferences(BinaryString other)
        {
            return Number.GetBitDifferences(other.Number);
        }
    }
}
