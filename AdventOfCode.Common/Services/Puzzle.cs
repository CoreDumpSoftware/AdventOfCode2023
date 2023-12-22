using System.Diagnostics;

namespace AdventOfCode.Api.Services;

public interface Puzzle
{
    Task<PuzzleResult> Solve(int part, IEnumerable<string> puzzleInput = null!);
}

public class PuzzleResult(long answer, TimeSpan elapsed)
{
    public long Answer { get; init; } = answer;
    public TimeSpan Elapsed { get; init; } = elapsed;

    public override string ToString() => $"Answer: {Answer}. Elapsed: {Elapsed}";
}

public abstract class PuzzleBase(IInputProvider inputProvider) : Puzzle
{
    protected abstract int _year { get; init; }
    protected abstract int _day { get; init; }

    protected readonly IEnumerable<string> Input;
    protected readonly IInputProvider _inputProvider = inputProvider;

    public async Task<PuzzleResult> Solve(int part, IEnumerable<string> puzzleInput = null!)
    {
        List<string> input;
        if (puzzleInput == null || !puzzleInput.Any())
            input = (await _inputProvider.GetPuzzleInput(_year, _day)).ToList();
        else
            input = puzzleInput.ToList();

        var stopwatch = Stopwatch.StartNew();
        long answer;

        try
        {
            answer = part switch
            {
                1 => PartOne(input),
                2 => PartTwo(input),
                _ => throw new ArgumentException($"Invalid part number.")
            };
        }
        finally
        {
            stopwatch.Stop();
        }

        return new(answer, stopwatch.Elapsed);
    }

    protected abstract long PartOne(IList<string> input);
    protected abstract long PartTwo(IList<string> input);
}