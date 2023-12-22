using AdventOfCode.Y2023.Day19.Rules;
using AdventOfCode.Y2023.Models;

namespace AdventOfCode.Y2023.Day19;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "19.txt";
    protected override string SampleInputOne { get; set; } = "19_sample.txt";

    public override long PartOne()
    {
        var data = GetFileContents(SolutionInput, true);
        var processingWorkflows = true;
        var workflows = new Dictionary<string, Workflow>();
        var parts = new List<Part>();

        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                processingWorkflows = false;
            }
            else if (processingWorkflows)
            {
                var workflow = Workflow.Parse(line);
                workflows[workflow.Name] = workflow;
            }
            else
            {
                parts.Add(Part.Parse(line));
            }
        }

        var sum = 0;

        foreach (var part in parts)
        {
            var result = ProcessWorkflow("in", workflows, part);
            sum += result;
        }

        return sum;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(SampleInputOne);
        var workflows = new Dictionary<string, Workflow>();

        foreach (var line in data)
        {
            if (string.IsNullOrEmpty(line))
                break;

            var workflow = Workflow.Parse(line);
            workflows[workflow.Name] = workflow;
        }

        var factory = new RuleTreeFactory(workflows);
        var root = factory.Create();

        PrintTreeDFS(root);

        var acceptPaths = new List<List<RulePath>>();
        GetAcceptPaths(root, acceptPaths);

        var sum = 0L;
        foreach (var path in acceptPaths)
        {
            var ranges = GetPartRangesFromRulePath(path);
            var rangeProduct = ranges.RangesProduct;
            sum += rangeProduct;
        }

        return sum;
    }

    public static int ProcessWorkflow(string start, Dictionary<string, Workflow> workflows, Part part)
    {
        var target = start;
        while (target != "A" && target != "R")
        {
            target = workflows[target].Process(part);
        }

        return target == "A"
            ? part.Sum
            : 0;
    }

    public static void PrintTreeDFS(BinaryNode<Rule> node, string path = null)
    {
        if (node.Left == null && node.Right == null)
        {
            Console.WriteLine(path + ", " + node);
        }

        if (node.Left != null)
        {
            var leftPath = path;
            if (string.IsNullOrEmpty(leftPath))
                leftPath = $"({node.Name})";
            else
                leftPath = leftPath + $", ({node.Name})";

            PrintTreeDFS(node.Left, leftPath);
        }

        if (node.Right != null)
        {
            var rightPath = path;
            if (string.IsNullOrEmpty(rightPath))
                rightPath = $"!({node.Name})"; // '!' indicates a negation on the condition
            else
                rightPath = rightPath + $", !({node.Name})";

            PrintTreeDFS(node.Right, rightPath);
        }
    }

    // not the best name... but meh.
    private record RulePath(Rule Rule, bool IsTrue);

    private static void GetAcceptPaths(BinaryNode<Rule> node, List<List<RulePath>> acceptRulePaths, List<RulePath> rules = null)
    {
        if (node.Name == "A")
        {
            acceptRulePaths.Add(rules);
            return;
        }

        if (node.Name == "R")
            return;

        var nextRules = rules?.ToList() ?? new List<RulePath>();

        if (node.Left != null)
        {
            nextRules.Add(new(node.Data, true));
            GetAcceptPaths(node.Left, acceptRulePaths, nextRules);
        }

        nextRules = rules?.ToList() ?? new List<RulePath>();

        if (node.Right != null)
        {
            nextRules.Add(new(node.Data, false));
            GetAcceptPaths(node.Right, acceptRulePaths, nextRules);
        }
    }

    private static PartRanges GetPartRangesFromRulePath(List<RulePath> rulePath)
    {
        var ranges = new PartRanges();

        foreach (var rule in rulePath)
        {
            rule.Rule.UpdateRanges(ranges, rule.IsTrue);
        }

        return ranges;
    }
}
