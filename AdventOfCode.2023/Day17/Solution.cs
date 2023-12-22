namespace AdventOfCode.Y2023.Day17;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "17.txt";
    protected override string SampleInputOne { get; set; } = "17_sample.txt";
    protected override string SampleInputTwo { get; set; } = "17_sample2.txt";

    public override long PartOne()
    {
        //var matrix = new Matrix<int>(GetFileContents(SampleInputOne)
        //    .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
        //    .ToArray());

        //var matrix = GetFileContents(SampleInputOne)
        //    .Select(l => l.ToCharArray().Select(c => c - '0').ToArray())
        //    .ToArray();

        //var start = new Position(0, 0);
        //var end = new Position(
        //    matrix.HorizontalBounds.Length - 1,
        //    matrix.VerticalBounds.Length - 1
        //);

        //var result = FindPath(matrix, start, end);


        return 0;
    }

    public override long PartTwo()
    {
        return 0;
    }

    private void Print2D(int[,] graph, int height, int width)
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(graph[x, y]);
            }

            Console.WriteLine();
        }
    }

    //private int FindDijkstras(Matrix<int> matrix,  Position start, Position end)
    //{
    //    var queue = new PriorityQueue<Path, int>();
    //    var startNode = new Path { DistanceToEnd = start.GetDistanceTo(end),  }
    //    matrix.GetAdjacentValues(start, true).Select(v => Path.CreatePathFromAdjacent(v, new ))

    //    return 0;
    //}

    //private int FindPath(Matrix<int> matrix, Position start, Position end)
    //{
    //    // Positions and the shortest path found to them
    //    var visitedNodes = new Dictionary<Position, int>
    //    {
    //        { start, 0 }
    //    };

    //    var stack = new List<Path>
    //    {
    //        new() { Position = new(0, 1), HeatLoss = 3, DistanceToEnd = new Position(0, 1).GetDistanceTo(end), Previous = [ start ] },
    //        new() { Position = new(1, 0), HeatLoss = 4, DistanceToEnd = new Position(1, 0).GetDistanceTo(end), Previous = [ start ] },
    //    };

    //    // debug variable to know how large the stack got at its worst
    //    var deepestStack = 0;

    //    while (stack.Count > 0)
    //    {
    //        var current = stack[0];
    //        stack.Remove(current);

    //        // Retrieves the next possible paths
    //        var adjacents = matrix.GetAdjacentValues(current.Position, true)
    //            .Where(n => !current.Previous.Contains(n))
    //            .Select(n => Path.CreatePathFromAdjacent(n, current, end))
    //            .Where(p => p.VerifyContiguousPathing());

    //        foreach (var node in adjacents)
    //        {
    //            if (node.Position == end)
    //                PrintPath(node, matrix.VerticalBounds.Length, matrix.HorizontalBounds.Length);

    //            var added = false;

    //            // If the node is missing from visited locations, add it and record the current shortest path to it
    //            if (!visitedNodes.TryGetValue(node.Position, out var shortestPath))
    //            {
    //                visitedNodes.Add(node.Position, node.Total);

    //                var insertIndex = FindInsertIndex(stack, node);
    //                stack.Insert(insertIndex, node);
    //            }
    //            // Otherwise lookup the known shortest and update if necessary.
    //            else if (node.Total < shortestPath)
    //            {
    //                var found = stack.Find(p => p.Position == node.Position);
    //                if (found != null)
    //                    stack.Remove(found);

    //                var insertIndex = FindInsertIndex(stack, node);
    //                stack.Insert(insertIndex, node);
    //            }

    //            if (stack.Count > deepestStack)
    //                deepestStack = stack.Count;
    //        }
    //    }

    //    return visitedNodes[end];
    //}

    //private static void PrintPath(Path path, int height, int width)
    //{
    //    var lines = Enumerable.Range(0, height).Select(y => new string(Enumerable.Repeat('.', width).ToArray())).ToList();
    //    for (var i = path.Previous.Count - 1; i >= 0; i--)
    //    {
    //        var current = i == 0
    //            ? path.Position
    //            : path.Previous[i - 1];
    //        var previous = path.Previous[i];

    //        var direction = previous.GetDirectionTo(current);
    //        var c = direction.ToSingleCharString();

    //        lines[current.Y] = lines[current.Y].ReplaceAt(current.X, 1, c.ToString());
    //    }

    //    Console.WriteLine($"Heat loss: {path.HeatLoss}");
    //    Console.WriteLine(string.Join("\n", lines));
    //    Console.WriteLine();
    //}

    //private static int FindInsertIndex(List<Path> stack, Path toInsert)
    //{
    //    if (stack.Count == 0)
    //        return 0;

    //    var index = stack.Count / 2;
    //    var nextIndexModifier = index / 2;
    //    while (nextIndexModifier != 0)
    //    {
    //        var diff = toInsert.Total.CompareTo(stack[index].Total);
    //        if (diff == 0)
    //        {
    //            while (index < stack.Count && stack[index].Total != toInsert.Total)
    //            {
    //                index++;
    //            }

    //            break;
    //        }
    //        else if (diff == -1)
    //        {
    //            index -= nextIndexModifier;
    //        }
    //        else if (diff == 1)
    //        {
    //            index += nextIndexModifier;
    //        }

    //        nextIndexModifier /= 2;
    //    }

    //    return index;
    //}
}

//public class Path
//{
//    public Position Position { get; init; }
//    public List<Position> Previous { get; set; } = new();

//    public int HeatLoss { get; set; }
//    public int DistanceToEnd { get; set; }
//    public int Total => HeatLoss + DistanceToEnd;

//    public override string ToString() =>
//        $"{Position}: {HeatLoss}+{DistanceToEnd}={Total}";

//    public static Path CreatePathFromAdjacent(ValuePosition<int> node, Path top, Position end)
//    {
//        var previous = new List<Position> { top.Position };
//        previous.AddRange(top.Previous);

//        return new Path
//        {
//            Position = node,
//            HeatLoss = node.Value + top.HeatLoss,
//            Previous = previous,
//            DistanceToEnd = node.GetDistanceTo(end)
//        };
//    }

//    public bool VerifyContiguousPathing()
//    {
//        if (Previous.Count < 4)
//            return true;

//        var next = Position.GetDirectionTo(Previous[0]!);
//        var firstPrev = Previous[0]!.GetDirectionTo(Previous[1]!);
//        var secondPrev = Previous[1]!.GetDirectionTo(Previous[2]!);
//        var thirdPrev = Previous[2]!.GetDirectionTo(Previous[3]!);

//        return new[] { next, firstPrev, secondPrev, thirdPrev }.Distinct().Count() > 1;
//    }
//}


//public class PathComparer : IComparer<Path>
//{
//    public int Compare(Path? x, Path? y)
//    {
//        if (x == null && y == null)
//            return 0;

//        if (x == null)
//            return -1;
//        if (y == null)
//            return 1;

//        return x.Total.CompareTo(y.Total);
//    }
//}