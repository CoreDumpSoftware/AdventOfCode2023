using AdventOfCode2023.Day19.Rules;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day19;

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
