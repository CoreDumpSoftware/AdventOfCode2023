using AdventOfCode2023.Day10;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day17;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "17.txt";
    protected override string SampleInputOne { get; set; } = "17_sample.txt";
    protected override string SampleInputTwo { get; set; } = "17_sample2.txt";

    public override long PartOne()
    {
        var matrix = new Matrix<int>(GetFileContents(SampleInputOne)
            .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
            .ToArray());

        var start = new Position(0, 0);
        var end = new Position(
            matrix.HorizontalBounds.Length - 1,
            matrix.VerticalBounds.Length - 1
        );

        var result = FindPath(matrix, start, end);

        return result;
    }

    public override long PartTwo()
    {
        return 0;
    }

    private int FindPath(Matrix<int> matrix, Position start, Position end)
    {
        // Positions and the shortest path found to them
        var visitedNodes = new Dictionary<Position, int>
        {
            { start, 0 }
        };

        var stack = new List<Path>();
        var startingNodes = matrix.GetAdjacentValues(start, true)
            .Where(n => n != start)
            .Select(n => new Path
            {
                Position = n,
                HeatLoss = n.Value,
                DistanceToEnd = n.GetDistanceTo(end),
                Previous = [start]
            });

        // Initialize the starting nodes for the stack to start looping through
        foreach (var node in startingNodes)
        {
            var index = FindInsertIndex(stack, node);
            stack.Insert(index, node);
            visitedNodes.Add(node.Position, node.Total);
        }

        // debug variable to know how large the stack got at its worst
        var deepestStack = 0;

        while (stack.Count > 0)
        {
            var current = stack[0];
            stack.Remove(current);

            // Retrieves the next possible paths
            var adjacents = matrix.GetAdjacentValues(current.Position, true)
                .Where(n => !current.Previous.Contains(n))
                .Select(n => CreatePathFromAdjacent(n, current, end))
                .Where(VerifyContiguousPathing);

            foreach (var node in adjacents)
            {
                if (node.Position == end)
                    PrintPath(node, matrix.VerticalBounds.Length, matrix.HorizontalBounds.Length);

                var added = false;

                // If the node is missing from visited locations, add it and record the current shortest path to it
                if (!visitedNodes.TryGetValue(node.Position, out var shortestPath))
                {
                    visitedNodes.Add(node.Position, node.Total);

                    //var insertIndex = FindInsertIndex(stack, node);
                    //stack.Insert(insertIndex, node);

                    stack.Add(node);
                    added = true;
                    //stack.OrderBy(n => n.Total);
                }
                // Otherwise lookup the known shortest and update if necessary.
                else if (node.Total < shortestPath)
                {
                    var found = stack.Find(p => p.Position == node.Position);
                    if (found != null)
                        stack.Remove(found);

                    //visitedNodes[node.Position] = node.Total;
                    //var insertIndex = FindInsertIndex(stack, node);
                    //stack.Insert(insertIndex, node);
                    stack.Add(node);
                    added = true;
                }

                //stack.OrderBy(n => n.Total);

                // Insert stuff commented above since it seems like my FindInsertIndex is buggy
                // Using a trusted sort didn't give a different answer though...
                if (added)
                    stack.Sort(new PathComparer());

                if (stack.Count > deepestStack)
                    deepestStack = stack.Count;
            }
        }

        return visitedNodes[end];
    }

    // Create a new path with history of where it traveled from
    private static Path CreatePathFromAdjacent(ValuePosition<int> node, Path top, Position end)
    {
        var previous = new List<Position> { top.Position };
        previous.AddRange(top.Previous);

        return new Path
        {
            Position = node,
            HeatLoss = node.Value + top.HeatLoss,
            Previous = previous,
            DistanceToEnd = node.GetDistanceTo(end)
        };
    }

    // Check the current and last 4 previous positions to determine the directions taken
    private static bool VerifyContiguousPathing(Path path)
    {
        if (path.Previous.Count < 4)
            return true;

        var next = path.Position.GetDirectionTo(path.Previous[0]!);
        var firstPrev = path.Previous[0]!.GetDirectionTo(path.Previous[1]!);
        var secondPrev = path.Previous[1]!.GetDirectionTo(path.Previous[2]!);
        var thirdPrev = path.Previous[2]!.GetDirectionTo(path.Previous[3]!);

        return new[] { next, firstPrev, secondPrev, thirdPrev }.Distinct().Count() > 1;
    }

    private static void PrintPath(Path path, int height, int width)
    {
        var lines = Enumerable.Range(0, height).Select(y => new string(Enumerable.Repeat('.', width).ToArray())).ToList();
        for (var i = path.Previous.Count - 1; i >= 0; i--)
        {
            var current = i == 0
                ? path.Position
                : path.Previous[i - 1];
            var previous = path.Previous[i];

            var direction = previous.GetDirectionTo(current);
            var c = direction.ToSingleCharString();

            lines[current.Y] = lines[current.Y].ReplaceAt(current.X, 1, c.ToString());
        }

        Console.WriteLine($"Heat loss: {path.HeatLoss}");
        Console.WriteLine(string.Join("\n", lines));
        Console.WriteLine();
    }

    private static int FindInsertIndex(List<Path> stack, Path toInsert)
    {
        if (stack.Count == 0)
            return 0;

        var index = stack.Count / 2;
        var nextIndexModifier = index / 2;
        while (nextIndexModifier != 0)
        {
            var diff = toInsert.Total.CompareTo(stack[index].Total);
            if (diff == 0)
            {
                while (index < stack.Count && stack[index].Total != toInsert.Total)
                {
                    index++;
                }

                break;
            }
            else if (diff == -1)
            {
                index -= nextIndexModifier;
            }
            else if (diff == 1)
            {
                index += nextIndexModifier;
            }

            nextIndexModifier /= 2;
        }

        return index;
    }
}

public class Path
{
    public Position Position { get; init; }
    public List<Position> Previous { get; set; } = new();

    public int HeatLoss { get; set; }
    public int DistanceToEnd { get; set; }
    public int Total => HeatLoss + DistanceToEnd;

    public override string ToString() =>
        $"{Position}: {HeatLoss}+{DistanceToEnd}={Total}";
}


public class PathComparer : IComparer<Path>
{
    public int Compare(Path? x, Path? y)
    {
        if (x == null && y == null)
            return 0;

        if (x == null)
            return -1;
        if (y == null)
            return 1;

        return x.Total.CompareTo(y.Total);
    }
}