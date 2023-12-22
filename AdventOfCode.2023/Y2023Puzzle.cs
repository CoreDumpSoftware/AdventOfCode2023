using AdventOfCode.Api.Services;

namespace AdventOfCode.Y2023;

public abstract class Y2023Puzzle(IInputProvider inputProvider) : PuzzleBase(inputProvider)
{
    protected sealed override int _year { get; init; } = 2023;
}