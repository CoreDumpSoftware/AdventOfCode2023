using System.Text.RegularExpressions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day19;

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
        var data = GetFileContents(SolutionInput);
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

        //PrintTreeDFS(root);

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
                rightPath = $"!({node.Name})";
            else
                rightPath = rightPath + $", !({node.Name})";

            PrintTreeDFS(node.Right, rightPath);
        }
    }

    public record RulePath(Rule Rule, bool IsTrue);

    public static void GetAcceptPaths(BinaryNode<Rule> node, List<List<RulePath>> acceptRulePaths, List<RulePath> rules = null)
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

public class RuleTreeFactory
{
    private readonly Dictionary<string, Workflow> _workflows;

    private readonly BinaryNode<Rule> _accept = new()
    {
        Name = "A",
    };

    private readonly BinaryNode<Rule> _reject = new()
    {
        Name = "R",
    };

    public RuleTreeFactory(Dictionary<string, Workflow> workflows)
    {
        _workflows = workflows;
    }

    public BinaryNode<Rule> Create()
    {
        var start = _workflows["in"];
        return BuildTree(start, 0);
    }

    private BinaryNode<Rule> BuildTree(Workflow workflow, int ruleIndex)
    {
        var node = new BinaryNode<Rule>
        {
            Name = $"{workflow.Name}-{ruleIndex}: {workflow.Rules[ruleIndex]}",
            Data = workflow.Rules[ruleIndex]
        };

        // Left traversal
        if (node.Data.TargetName == "A")
            node.Left = _accept;
        else if (node.Data.TargetName == "R")
            node.Left = _reject;
        else
            node.Left = BuildTree(_workflows[node.Data.TargetName], 0);

        // Right traversal
        var nextRule = workflow.Rules[ruleIndex + 1];
        if (nextRule.GetType() == typeof(Rule)) // node jump instruction
        {
            node.Right = nextRule.TargetName switch
            {
                "A" => node.Right = _accept,
                "R" => node.Right = _reject,
                _ => node.Right = BuildTree(_workflows[nextRule.TargetName], 0)
            };
        }
        else
        {
            node.Right = BuildTree(workflow, ruleIndex + 1);
        }

        return node;
    }
}

[Flags]
public enum Operator
{
    LessThan = 1,
    GreaterThan = 2,
    Equal = 4
}

public class Workflow
{
    public string Name { get; init; }
    public IReadOnlyList<Rule> Rules { get; init; }

    private Workflow() { }

    public virtual string Process(Part part)
    {
        foreach (var rule in Rules)
        {
            if (rule.Check(part))
                return rule.TargetName;
        }

        return "R";
    }

    public static Workflow Parse(string line)
    {
        var index = line.IndexOf('{');

        var name = line[..index];

        var regex = new Regex(@"((?'Category'\w+)(?'Operator'\<|\>)(?'Amount'\d+):)?(?'Target'\w+)");
        var rulesString = line[(index + 1)..^1];
        var rules = new List<Rule>();

        foreach (Match match in regex.Matches(rulesString))
        {
            var category = match.Groups["Category"].Value;
            var op = match.Groups["Operator"].Value;
            int.TryParse(match.Groups["Amount"].Value, out var amount);
            var target = match.Groups["Target"].Value;

            var rule = category switch
            {
                "x" => new XRule { Operator = op, Amount = amount, TargetName = target },
                "m" => new MRule { Operator = op, Amount = amount, TargetName = target },
                "a" => new ARule { Operator = op, Amount = amount, TargetName = target },
                "s" => new SRule { Operator = op, Amount = amount, TargetName = target },
                  _ =>  new Rule { TargetName = target }
            };

            rules.Add(rule);
        }

        return new()
        {
            Name = name,
            Rules = rules
        };
    }
}

// values are inclusive: Min >= n <= Max
public class SimpleRange
{
    public int Min { get; set; }
    public int Max { get; set; }
    public int Size => Max - Min + 1; // e.g., 1415 - 100 + 1 = 1315

    public SimpleRange()
    {
        Min = PartRanges.AbsoluteMinRange;
        Max = PartRanges.AbsoluteMaxRange;
    }

    public SimpleRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString() => $"{Min} >= n <= {Max}";
}

public class PartRanges
{
    public const int AbsoluteMinRange = 1;
    public const int AbsoluteMaxRange = 4000;

    public SimpleRange X { get; set; } = new();
    public SimpleRange M { get; set; } = new();
    public SimpleRange A { get; set; } = new();
    public SimpleRange S { get; set; } = new();

    public long RangesProduct => (long)X.Size * (long)M.Size * (long)A.Size * (long)S.Size;
}

public class Part
{
    public int X { get; init; }
    public int M { get; init; }
    public int A { get; init; }
    public int S { get; init; }

    public int Sum => X + M + A + S;

    private Part() { }

    public static Part Parse(string line)
    {
        var variables = new int[4];
        var regex = new Regex(@"\d+");
        var index = 0;
        foreach (Match match in regex.Matches(line))
        {
            variables[index++] = int.Parse(match.Value);
        }

        return new()
        {
            X = variables[0],
            M = variables[1],
            A = variables[2],
            S = variables[3]
        };
    }
}

public class Rule
{
    protected readonly string? _operator;

    public string? Operator {
        get => _operator;
        init
        {
            _operator = value;
            if (_operator != null)
            {
                _compareFn = _operator == "<"
                    ? LessThan
                    : GreaterThan;
            }
        }
    }
    public int? Amount { get; init; }

    protected Func<int, bool> _compareFn = _ => true;

    // When the condition is met, execute the next rule
    public string TargetName { get; init; }

    public virtual bool Check(Part input) => true;

    private bool LessThan(int value) => value < Amount;
    private bool GreaterThan(int value) => value > Amount;

    public override string ToString() => GetType() == typeof(Rule) ? TargetName : GetExpressionString();

    protected virtual string _condition { get; set; } = null!;
    protected virtual string GetExpressionString() => $"{_condition.ToLower()}{_operator}{Amount}";

    public virtual void UpdateRanges(PartRanges ranges, bool isTrue)
    {
    }
}

public class XRule : Rule
{
    protected override string _condition { get; set; } = "X";

    public override bool Check(Part input) => _compareFn(input.X);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.X.Max > Amount - 1)
            {
                ranges.X.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.X.Min < Amount + 1)
            {
                ranges.X.Min = Amount.Value + 1;
            }
        }
        else // invert the operator
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.X.Min < Amount)
            {
                ranges.X.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.X.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.X.Max = Amount.Value;
            }
        }
    }
}

public class MRule : Rule
{
    protected override string _condition { get; set; } = "M";

    public override bool Check(Part input) => _compareFn(input.M);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.M.Max > Amount - 1)
            {
                ranges.M.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.M.Min < Amount + 1)
            {
                ranges.M.Min = Amount.Value + 1;
            }
        }
        else
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.M.Min < Amount)
            {
                ranges.M.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.M.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.M.Max = Amount.Value;
            }
        }
    }
}

public class ARule : Rule
{
    protected override string _condition { get; set; } = "A";

    public override bool Check(Part input) => _compareFn(input.A);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            if (Operator == "<" && ranges.A.Max > Amount - 1)
            {
                ranges.A.Max = Amount.Value - 1;
            }
            else if (Operator == ">" && ranges.A.Min < Amount + 1)
            {
                ranges.A.Min = Amount.Value + 1;
            }
        }
        else
        { // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.A.Min < Amount)
            {
                ranges.A.Min = Amount.Value;
            }
            else if (Operator == ">" && ranges.A.Max > Amount)
            { // s > 1351 --> s <= 1351
                ranges.A.Max = Amount.Value;
            }
        }
    }
}

public class SRule : Rule
{
    protected override string _condition { get; set; } = "S";

    public override bool Check(Part input) => _compareFn(input.S);

    public override void UpdateRanges(PartRanges ranges, bool isTrue)
    {
        if (isTrue)
        {
            // s < 2000 --> 1..1999
            if (Operator == "<" && ranges.S.Max > Amount - 1)
            {
                ranges.S.Max = Amount.Value - 1;
            }
            // s > 2000 --> 2001..4000
            else if (Operator == ">" && ranges.S.Min < Amount + 1)
            {
                ranges.S.Min = Amount.Value + 1;
            }
        }
        else
        {
            // s < 1351 --> s >= 1351
            if (Operator == "<" && ranges.S.Min < Amount)
            {
                ranges.S.Min = Amount.Value;
            }
            // s > 1351 --> s <= 1351
            else if (Operator == ">" && ranges.S.Max > Amount)
            {
                ranges.S.Max = Amount.Value;
            }
        }
    }
}