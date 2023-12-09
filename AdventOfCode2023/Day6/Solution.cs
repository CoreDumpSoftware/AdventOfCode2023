using System.Net.Mime;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day6;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "6_1.txt";
    protected override string PartTwoInputFile { get; init; } = "6_1_sample.txt";

    public override long PartOne()
    {
        var races = GetRacesPartOne(GetFileContents(PartOneInputFile)).ToArray();
        long product = 0;

        foreach (var race in races)
        {
            var recordBeatingCount = 0;
            for (var time = 1; time < race.Record; time++)
            {
                var distance = time * (race.Time - time);
                if (distance > race.Record)
                    recordBeatingCount++;
            }

            if (product == 0)
                product = recordBeatingCount;
            else
                product *= recordBeatingCount;
        }

        return product;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile);
        var timeLine = data.First();
        var recordLine = data.Skip(1).First();

        var timeNumber = long.Parse(timeLine[6..].Replace(" ", string.Empty));
        var raceRecord = long.Parse(recordLine[10..].Replace(" ", string.Empty));

        var race = new Race(timeNumber, raceRecord);

        // find the first value that exceeds the limit...
        long lowerInvalid = 0;
        for (long time = 0; time < race.Time; time++)
        {
            var distance = time * (race.Time - time);
            if (distance <= raceRecord)
                lowerInvalid++;
            else
                break;
        }

        long upperInvalid = 0;
        for (long time = race.Time - 1; time >= 0; time--)
        {
            var distance = time * (race.Time - time);
            if (distance <= raceRecord)
                upperInvalid++;
            else
                break;
        }

        var result = race.Time - lowerInvalid - upperInvalid;

        return 0;
    }

    private record Race(long Time, long Record);

    private static IEnumerable<Race> GetRacesPartOne(IEnumerable<string> input)
    {
        var timeLine = input.First();
        var recordsLine = input.Skip(1).First();

        var regex = new Regex(@"\d+");

        var times = regex.Matches(timeLine).Select(g => long.Parse(g.Value)).ToArray();
        var records = regex.Matches(recordsLine).Select(g => long.Parse(g.Value)).ToArray();

        foreach (var pairs in times.Zip(records))
            yield return new Race(pairs.First, pairs.Second);
    }
}
