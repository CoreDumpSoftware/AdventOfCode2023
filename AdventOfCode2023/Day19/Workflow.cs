using System.Text.RegularExpressions;
using AdventOfCode2023.Day19.Rules;

namespace AdventOfCode2023.Day19;

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
