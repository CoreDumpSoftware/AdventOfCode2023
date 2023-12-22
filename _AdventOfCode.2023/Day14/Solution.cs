using System.Collections.Concurrent;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day14;

public class Solution : SolutionBase
{
    private const int MultiThreadingChunkSize = 8; // 100 / 8 = 12; even workload for threads

    protected override string SolutionInput { get; init; } = "14.txt";
    protected override string SampleInputOne { get; set; } = "14_sample_1.txt";

    public override long PartOne()
    {
        var platform = GetFileContents(SolutionInput).Select(l => l.ToCharArray()).ToArray();
        int width = platform[0].Length;
        int height = platform.Length;

        TiltPlatform(platform, height, width, Direction.North, true);
        var result = CalculateLoadFromDirection(platform, height, width, Direction.North);

        return result;
    }

    private static int Hash(char[][] matrix, int height, int width)
    {
        var hash = 17;
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                hash = hash * 31 + matrix[y][x];

        return hash;
    }

    private class Pattern
    {
        public char[][] Platform { get; set; }
        public int Hash { get; set; }
        public int Load { get; set; }
        public List<int> Indices { get; set; }

        public Pattern(char[][] platform, int hash, int sourceIndex, int load, int height, int width)
        {
            Platform = platform.Select(r => r.ToArray()).ToArray();
            Hash = hash;
            Indices = [sourceIndex];
            Load = load;
        }
    }

    public override long PartTwo()
    {
        var platform = GetFileContents(SolutionInput).Select(l => l.ToCharArray()).ToArray();
        int width = platform[0].Length;
        int height = platform.Length;

        var hashDictionary = new Dictionary<int, Pattern>();

        var intervals = 10000;

        for (var i = 0; i < intervals; i++)
        {
            if (i % 10000 == 0)
                Console.WriteLine(((double)i / (double)intervals * (double)100) + "%");

            TiltPlatform(platform, height, width, Direction.North, true);
            TiltPlatform(platform, height, width, Direction.West, true);
            TiltPlatform(platform, height, width, Direction.South, true);
            TiltPlatform(platform, height, width, Direction.East, true);

            var hash = Hash(platform, height, width);
            if (hashDictionary.TryGetValue(hash, out var pattern))
                pattern.Indices.Add(i);
            else
            {
                pattern = new Pattern(platform, hash, i, CalculateLoadFromDirection(platform, height, width, Direction.North), height, width);

                hashDictionary.Add(hash, pattern);
            }
        }

        var whatever = hashDictionary
            .Select(kvp => kvp.Value)
            .Where(p => p.Indices.Count > 1)
            .Select(p => new
            {
                FirstIndex = p.Indices[0],
                p.Load,
                BillionDiff = 1000000000 % p.Indices[0],
                Indices = p.Indices
            })
            .ToArray();

        // do some math here to find the right answer
        // There's probably some fancy math to do this without
        // guess and check.

        // I essentially just filtered out the patterns where the indices were all odd since
        // my solution produced a rotation of 22 different patterns after the initial chaos
        // was sorted out. From there it was pretty much looking at the starting index and
        // figuring out which one would eventually go to a billion. Lots of n + (22 * m)
        // entries in a calculator :-)

        return 0;
    }

    private static void TiltPlatform(char[][] platform, int height, int width, Direction direction, bool parallel)
    {
        if (direction == Direction.North)
        {
            TiltNorth(platform, height, width, parallel);
        }
        else if (direction == Direction.South)
        {
            TiltSouth(platform, height, width, parallel);
        }
        else if (direction == Direction.West)
        {
            TiltWest(platform, height, width, parallel);
        }
        else if (direction == Direction.East)
        {
            TiltEast(platform, height, width, parallel);
        }
    }

    private static void TiltEast(char[][] platform, int height, int width, bool parallel)
    {
        if (parallel)
        {
            var rangePartitioner = Partitioner.Create(0, height, MultiThreadingChunkSize);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (var y = range.Item1; y < range.Item2; y++)
                {
                    var rockIndex = width;
                    var rockOffset = 1;

                    for (var x = width - 1; x >= 0; x--)
                    {
                        switch (platform[y][x])
                        {
                            case '#':
                                rockIndex = x;
                                rockOffset = 1;
                                break;
                            case 'O':
                                if (x != rockIndex - rockOffset)
                                {
                                    platform[y][rockIndex - rockOffset] = 'O';
                                    platform[y][x] = '.';
                                }

                                rockOffset++;

                                break;
                        }
                    }
                }
            });
        }
        else
        {
            for (var y = 0; y < height; y++)
            {
                var rockIndex = width;
                var rockOffset = 1;

                for (var x = width - 1; x >= 0; x--)
                {
                    switch (platform[y][x])
                    {
                        case '#':
                            rockIndex = x;
                            rockOffset = 1;
                            break;
                        case 'O':
                            if (x != rockIndex - rockOffset)
                            {
                                platform[y][rockIndex - rockOffset] = 'O';
                                platform[y][x] = '.';
                            }

                            rockOffset++;

                            break;
                    }
                }
            };
        }
    }

    private static void TiltWest(char[][] platform, int height, int width, bool parallel)
    {
        if (parallel)
        {
            var rangePartitioner = Partitioner.Create(0, height, MultiThreadingChunkSize);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (var y = range.Item1; y < range.Item2; y++)
                {
                    var rockIndex = -1;
                    var rockOffset = 1;

                    for (var x = 0; x < width; x++)
                    {
                        switch (platform[y][x])
                        {
                            case '#':
                                rockIndex = x;
                                rockOffset = 1;
                                break;
                            case 'O':
                                if (x != rockIndex + rockOffset)
                                {
                                    platform[y][rockIndex + rockOffset] = 'O';
                                    platform[y][x] = '.';
                                }

                                rockOffset++;

                                break;
                        }
                    }
                }
            });
        }
        else
        {
            for (var y = 0; y < height; y++)
            {
                var rockIndex = -1;
                var rockOffset = 1;

                for (var x = 0; x < width; x++)
                {
                    switch (platform[y][x])
                    {
                        case '#':
                            rockIndex = x;
                            rockOffset = 1;
                            break;
                        case 'O':
                            if (x != rockIndex + rockOffset)
                            {
                                platform[y][rockIndex + rockOffset] = 'O';
                                platform[y][x] = '.';
                            }

                            rockOffset++;

                            break;
                    }
                }
            };
        }
    }

    private static void TiltSouth(char[][] platform, int height, int width, bool parallel)
    {
        if (parallel)
        {
            var rangePartitioner = Partitioner.Create(0, width, MultiThreadingChunkSize);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (var x = range.Item1; x < range.Item2; x++)
                {
                    var rockIndex = height;
                    var rockOffset = 1;

                    for (var y = width - 1; y >= 0; y--)
                    {
                        switch (platform[y][x])
                        {
                            case '#':
                                rockIndex = y;
                                rockOffset = 1;
                                break;
                            case 'O':
                                if (y != rockIndex - rockOffset)
                                {
                                    platform[rockIndex - rockOffset][x] = 'O';
                                    platform[y][x] = '.';
                                }

                                rockOffset++;

                                break;
                        }
                    }
                }
            });
        }
        else
        {
            for (var x = 0; x < width; x++)
            {
                var rockIndex = height;
                var rockOffset = 1;

                for (var y = width - 1; y >= 0; y--)
                {
                    switch (platform[y][x])
                    {
                        case '#':
                            rockIndex = y;
                            rockOffset = 1;
                            break;
                        case 'O':
                            if (y != rockIndex - rockOffset)
                            {
                                platform[rockIndex - rockOffset][x] = 'O';
                                platform[y][x] = '.';
                            }

                            rockOffset++;

                            break;
                    }
                }
            };
        }
    }

    private static void TiltNorth(char[][] platform, int height, int width, bool parallel)
    {
        if (parallel)
        {
            var rangePartitioner = Partitioner.Create(0, width, MultiThreadingChunkSize);
            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (var x = range.Item1; x < range.Item2; x++)
                {
                    var rockIndex = -1;
                    var rockOffset = 1;

                    for (var y = 0; y < height; y++)
                    {
                        switch (platform[y][x])
                        {
                            case '#':
                                rockIndex = y;
                                rockOffset = 1;
                                break;
                            case 'O':
                                if (y != rockIndex + rockOffset)
                                {
                                    platform[rockIndex + rockOffset][x] = 'O';
                                    platform[y][x] = '.';
                                }

                                rockOffset++;

                                break;
                        }
                    }
                }
            });
        }
        else
        {
            for (var x = 0; x < width; x++)
            {
                var rockIndex = -1;
                var rockOffset = 1;

                for (var y = 0; y < height; y++)
                {
                    switch (platform[y][x])
                    {
                        case '#':
                            rockIndex = y;
                            rockOffset = 1;
                            break;
                        case 'O':
                            if (y != rockIndex + rockOffset)
                            {
                                platform[rockIndex + rockOffset][x] = 'O';
                                platform[y][x] = '.';
                            }

                            rockOffset++;

                            break;
                    }
                }
            };
        }
    }

    private static int CalculateLoadFromDirection(char[][] platform, int height, int width, Direction direction)
    {
        var sum = 0;
        for (var x = 0; x < width; x++)
        {
            var columnTotal = 0;
            for (var y = 0; y < height; y++)
            {
                if (platform[y][x] == 'O')
                    columnTotal += height - y;
            }

            sum += columnTotal;
        }

        return sum;
    }
}