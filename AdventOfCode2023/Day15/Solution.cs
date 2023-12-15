using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day15;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "15.txt";
    protected override string SampleInputOne { get; set; } = "15_sample.txt";

    private class Lens(string label, int focalLength)
    {
        public string Label { get; init; } = label;
        public int FocalLength { get; set; } = focalLength;

        public override string ToString() => $"[{Label} {FocalLength}]";
    }

    public override long PartOne()
    {
        var knownHashes = new Dictionary<string, int>();
        var line = GetFileContents(SolutionInput).First();
        using var stringReader = new StringReader(line);
        var token = stringReader.ReadUntil(',');
        var sum = 0;

        while (!string.IsNullOrEmpty(token.Value))
        {
            var result = CalculateHash(token.Value, knownHashes);
            sum += result;

            token = stringReader.ReadUntil(',');
        }

        return sum;
    }

    public override long PartTwo()
    {
        var line = GetFileContents(SolutionInput).First();
        using var stringReader = new StringReader(line);
        var token = stringReader.ReadUntil(',');

        var labelHashes = new Dictionary<string, int>();
        var lensBoxes = new Dictionary<int, List<Lens>>();

        while (!string.IsNullOrEmpty(token.Value))
        {
            var last = token.Value[^1];
            var isRemove = last == '-';
            var label = token.Value[..^(isRemove ? 1 : 2)];
            var labelHash = CalculateHash(label, labelHashes);

            // remove
            if (last == '-' && lensBoxes.TryGetValue(labelHash, out var box))
            {
                var lens = box.FirstOrDefault(l => l.Label == label);

                if (lens != null)
                    box.Remove(lens);
            }
            // add or update
            else
            {
                var focalLength = last - '0';

                if (!lensBoxes.TryGetValue(labelHash, out box))
                {
                    box = [new(label, focalLength)];
                    lensBoxes.Add(labelHash, box);
                }
                else
                {
                    var lens = box.FirstOrDefault(l => l.Label == label);

                    if (lens != null)
                        lens.FocalLength = focalLength;
                    else
                        box.Add(new(label, focalLength));
                }

            }

            token = stringReader.ReadUntil(',');
        }

        var sum = 0;
        foreach (var kvp in lensBoxes.Where(kvp => kvp.Value.Any()).OrderBy(kvp => kvp.Key))
        {
            for (var i = 0; i <  kvp.Value.Count; i++)
            {
                var result = (kvp.Key + 1) * (i + 1) * kvp.Value[i].FocalLength;
                sum += result;
            }
        }

        return sum;
    }

    public int CalculateHash(string input, Dictionary<string, int> knownHashes)
    {
        if (knownHashes.TryGetValue(input, out var hash))
            return hash;

        var result = 0;

        foreach (var c in input)
        {
            result += c;
            result *= 17;
            result %= 256;
        }

        knownHashes.Add(input, result);

        return result;
    }

}
