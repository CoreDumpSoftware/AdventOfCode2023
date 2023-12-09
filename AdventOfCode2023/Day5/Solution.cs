using System.Collections.Concurrent;
using System.Data;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day5;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "5_1.txt";
    protected override string PartTwoInputFile { get; init; } = "5_1_sample.txt";

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile).ToList();

        var seeds = data[0][(data[0].IndexOf(':') + 2)..].ParseLongs().ToList();

        var maps = ParseMaps(data);
        var locations = GetSeedLocations(seeds, maps);

        var result = (int)locations.Min();

        return result;
    }

    private static List<long> GetSeedLocations(List<long> seeds, List<Map> maps)
    {
        var locations = new List<long>();

        foreach (var seed in seeds)
        {
            var value = seed;

            foreach (var map in maps)
            {
                value = map[value];
            }

            locations.Add(value);
        }

        return locations;
    }

    private static List<Map> ParseMaps(List<string> data)
    {
        var maps = new List<Map>();
        MapDefinition currentMapping = null!;

        foreach (var line in data.Skip(1))
        {
            var index = line.IndexOf(" map");

            if (index > 0)
            {
                if (currentMapping != null)
                {
                    maps.Add(new Map(currentMapping));
                }

                var mappingName = line[..index];
                var parts = mappingName.Split('-');
                var srcName = parts[0];
                var dstName = parts[2];
                currentMapping = new(srcName, dstName, []);
            }
            else
            {
                currentMapping.Mappings.Add(line);
            }
        }

        maps.Add(new Map(currentMapping));


        return maps;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile).ToList();

        var seeds = ParsePartTwoSeeds(data[0]).OrderBy(x => x.Start).ToList();

        var maps = ParseMaps(data);

        object lockObject = new object();
        long lowestLocation = long.MaxValue;

        var dict = new ConcurrentDictionary<long, byte>();

        Parallel.ForEach(seeds, new ParallelOptions { MaxDegreeOfParallelism = 16 }, (Range seedRange) =>
        {
            long localLowest = long.MaxValue;
            long lastKnownGlobalLowest = long.MaxValue;
            for (long seed = seedRange.Start; seed <= seedRange.End; seed++)
            {
                long value = seed;
                foreach (var map in maps)
                {
                    value = map[value];
                }

                if (value >= localLowest)
                    continue;

                localLowest = value;

                if (localLowest < lastKnownGlobalLowest)
                {
                    lock (lockObject)
                    {
                        if (localLowest < lowestLocation)
                        {
                            lowestLocation = localLowest;
                        }

                        lastKnownGlobalLowest = lowestLocation;
                    }
                }
            }
        });

        return lowestLocation;
    }

    private IEnumerable<Range> ParsePartTwoSeeds(string line)
    {
        long start = -1;
        long length = -1;

        foreach (var value in line[(line.IndexOf(':') + 2)..].ParseLongs())
        {
            if (start == -1)
                start = value;
            else if (length == -1)
            {
                length = value;

                yield return new Range(start, length);

                start = -1;
                length = -1;
            }
        }
    }
}
