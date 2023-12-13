namespace AdventOfCode2023.Day12;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "12.txt";
    protected override string PartTwoInputFile { get; init; } = null!;
    protected override string SampleInputOne { get; set; } = "12_sample1.txt";

    private enum Status : byte
    {
        Undefined = 0,
        Operational = (byte)'.',
        Damaged = (byte)'#',
        Unknown = (byte)'?'
    }

    public override long PartOne()
    {
        var lines = GetFileContents(PartOneInputFile);
        var sum = 0L;
        var debug = true;
        var lineNumber = 1;

        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var groups = parts[1].Split(',').Select(int.Parse).ToArray();

            var resolver = new RowPermutations(parts[0], groups);
            var count = resolver.Count();

            sum += count;
        }

        return sum;
    }

    public override long PartTwo()
    {
        var lines = GetFileContents(PartOneInputFile);
        var sum = 0L;
        var debug = true;
        var lineNumber = 1;
        const int unfoldRepeatCount = 5;
        object fileLock = new object();
        var outputFile = "out.txt";
        var counts = new List<string>();

        //foreach (var line in lines)
        Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = 4 }, line =>
        {
            var parts = line.Split(' ');
            var row = string.Join("?", Enumerable.Repeat(parts[0], unfoldRepeatCount));

            var groups = Enumerable.Repeat(parts[1].Split(',').Select(int.Parse), unfoldRepeatCount)
                .SelectMany(x => x)
                .ToArray();

            if (debug)
            {
                Console.WriteLine($"{lineNumber++}:");
                Console.WriteLine($"Unfolded row: {row}");
                Console.WriteLine($"Unfolded groups: {string.Join(", ", groups)}");
            }

            var resolver = new RowPermutations(row, groups);
            int count = 0;
            foreach (var permutation in resolver)
            {
                count++;
            }

            if (debug)
                Console.WriteLine($"\tTotal: {count}\n");

            lock (fileLock)
            {
                counts.Add($"{line} --> {count}");
                File.WriteAllLines(outputFile, counts);
            }

            sum += count;
        });

        return sum;
    }

}
