using AdventOfCode.Y2023.Models;
using LPosition = (long X, long Y);

namespace AdventOfCode.Y2023.Day18;

public class Solution : SolutionBase
{
    private struct Extremes(long bottom, long top)
    {
        public long Bottom { get; set; } = bottom;
        public long Top { get; set; } = top;

        public readonly long Difference => Top - Bottom;
    }

    protected override string SolutionInput { get; init; } = "18.txt";
    protected override string SampleInputOne { get; set; } = "18_sample.txt";

    public override long PartOne()
    {
        var instructions = GetFileContents(SolutionInput)
            .Select(l => l.Split(' ')).Select(p => (Direction: GetDirection(p[0]), Distance: int.Parse(p[1])));

        var current = new LPosition(0, 0);
        var vertices = new List<LPosition> { current };
        var totalEdgeLength = 0L;

        foreach (var instruction in instructions)
        {
            switch (instruction.Direction)
            {
                case Direction.North:
                    current = new LPosition(current.X, current.Y - instruction.Distance);
                    break;
                case Direction.South:
                    current = new LPosition(current.X, current.Y + instruction.Distance);
                    break;
                case Direction.West:
                    current = new LPosition(current.X - instruction.Distance, current.Y);
                    break;
                case Direction.East:
                    current = new LPosition(current.X + instruction.Distance, current.Y);
                    break;
            }

            vertices.Add(current);
            totalEdgeLength += instruction.Distance;
        }

        var area = Shoelace.Solve(vertices, totalEdgeLength);

        return area;
    }

    public override long PartTwo()
    {
        var instructions = GetFileContents(SolutionInput)
            .Select(l => l.Split(' '))
            .Select(p => DecodeInstruction(p[2][2..^1]));

        var current = new LPosition(0, 0);
        var vertices = new List<LPosition> { current };
        var totalEdgeLength = 0L;

        foreach (var instruction in instructions)
        {
            switch (instruction.Direction)
            {
                case Direction.North:
                    current = new LPosition(current.X, current.Y - instruction.Distance);
                    break;
                case Direction.South:
                    current = new LPosition(current.X, current.Y + instruction.Distance);
                    break;
                case Direction.West:
                    current = new LPosition(current.X - instruction.Distance, current.Y);
                    break;
                case Direction.East:
                    current = new LPosition(current.X + instruction.Distance, current.Y);
                    break;
            }

            vertices.Add(current);
            totalEdgeLength += instruction.Distance;
        }

        var area = Shoelace.Solve(vertices, totalEdgeLength);

        return area;
    }

    private static (Direction Direction, long Distance) DecodeInstruction(string line)
    {
        var distance = Convert.ToInt64(line[..5], 16);
        var direction = GetDirection(line[5].ToString());

        return (direction, distance);
    }

    private static Direction GetDirection(string d) => string.IsNullOrEmpty(d)
        ? throw new ArgumentNullException(nameof(d))
        : char.ToUpper(d[0]) switch
        {
            'U' or 'N' or '3'  => Direction.North,
            'D' or 'S' or '1' => Direction.South,
            'L' or 'W' or '2' => Direction.West,
            'R' or 'E' or '0' => Direction.East,
            _ => throw new ArgumentException("Invalid direction character")
        };
}

public static class Shoelace
{
    public static long Solve(List<Position> vertices, long totalEdgeLength)
    {
        // https://en.wikipedia.org/wiki/Shoelace_formula
        // https://en.wikipedia.org/wiki/Pick%27s_theorem

        long area = 0;

        foreach ((var left, var right) in Enumerable.Range(0, vertices.Count - 1).Select(i => (Left: vertices[i], Right: vertices[i + 1])))
        {
            area += (left.Y + right.Y) * (left.X - right.X);
        }

        var first = vertices[0];
        var last = vertices[^1];

        area += (last.Y + first.Y) * (last.X - first.X);

        area = Math.Abs(area);
        area += totalEdgeLength;
        area /= 2;

        return area + 1;
    }

    public static long Solve(List<(long X, long Y)> vertices, long totalEdgeLength)
    {
        // https://en.wikipedia.org/wiki/Shoelace_formula
        // https://en.wikipedia.org/wiki/Pick%27s_theorem

        long area = 0;

        foreach ((var left, var right) in Enumerable.Range(0, vertices.Count - 1).Select(i => (Left: vertices[i], Right: vertices[i + 1])))
        {
            area += (left.Y + right.Y) * (left.X - right.X);
        }

        var first = vertices[0];
        var last = vertices[^1];

        area += (last.Y + first.Y) * (last.X - first.X);

        area = Math.Abs(area);
        area += totalEdgeLength;
        area /= 2;

        return area + 1;
    }
}