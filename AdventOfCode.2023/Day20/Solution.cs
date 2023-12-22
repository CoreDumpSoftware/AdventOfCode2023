using AdventOfCode.Api.Services;
using AdventOfCode.Y2023.Day20.Modules;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2023.Day20;

public class Solution(IInputProvider inputProvider, ILogger logger) : Y2023Puzzle(inputProvider)
{
    private readonly ModuleFactory _factory = new();
    private readonly ILogger _logger = logger;

    protected override int _day { get; init; } = 20;

    protected override long PartOne(IList<string> input)
    {
        var firstLineWasCount = int.TryParse(input[0], out var repeatCount);

        var button = _factory.CreateNetwork(
            firstLineWasCount ? input.Skip(1) : input,
            logLines => { });

        var result = button.Push(firstLineWasCount ? repeatCount : 1);

        return result;
    }

    protected override long PartTwo(IList<string> input)
    {
        var firstLineWasCount = int.TryParse(input[0], out var repeatCount);

        var button = _factory.CreateNetwork(
            firstLineWasCount ? input.Skip(1) : input,
            logLines => { });

        var pushCount = button.PushAndWaitForOutputTrigger();

        return pushCount;
    }
}
