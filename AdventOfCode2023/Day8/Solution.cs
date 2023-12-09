using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day8;

public class Solution : SolutionBase
{
    protected override string PartOneInputFile { get; init; } = "8_1.txt";
    protected override string PartTwoInputFile { get; init; } = "8_1_sample.txt";

    public override long PartOne()
    {
        var data = GetFileContents(PartOneInputFile);
        var directions = data.First();

        var directionsDictionary = data.Skip(1)
            .Select(l => new Node(l))
            .GroupBy(n => n.Value)
            .ToDictionary(g => g.Key, g => g.First());

        var currentNode = directionsDictionary["AAA"];
        var enumerator = new DirectionEnumerator(directions);
        long steps = 0;

        while (currentNode.Value != "ZZZ")
        {
            steps++;
            var direction = enumerator.Current;

            if (direction == -1)
            {
                var temp = currentNode;
                currentNode = currentNode.LeftNode ?? directionsDictionary[currentNode.Left];

                temp.LeftNode = currentNode;
            }
            else
            {
                var temp = currentNode;
                currentNode = currentNode.RightNode ?? directionsDictionary[currentNode.Right];

                temp.RightNode = currentNode;
            }

            enumerator.MoveNext();
        }

        return steps;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(PartOneInputFile);
        var directions = data.First();

        var nodeMap = data.Skip(1)
            .Select(l => new Node(l))
            .GroupBy(n => n.Value)
            .ToDictionary(g => g.Key, g => g.First());

        var nodes = nodeMap
            .Where(kvp => kvp.Key.EndsWith('A'))
            .Select(kvp => kvp.Value)
            .ToList();

        var enumerator = new DirectionEnumerator(directions);
        long steps = 0;

        var stepsCount = new long[nodes.Count()];

        for (var i = 0; i < nodes.Count(); i++)
        {
            var node = nodes[i];

            while (!node.Value.EndsWith('Z'))
            {
                var direction = enumerator.Current;
                stepsCount[i]++;

                if (direction == -1)
                {
                    node.LeftNode ??= nodeMap[node.Left];
                    node = node.LeftNode;
                }
                else
                {
                    node.RightNode ??= nodeMap[node.Right];
                    node = node.RightNode;
                }

                enumerator.MoveNext();
            }

            nodes[i] = node;
        }

        var result = stepsCount.LeastCommonMultiple();

        return result;
    }
}
