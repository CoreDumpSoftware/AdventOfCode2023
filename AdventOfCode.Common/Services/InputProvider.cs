namespace AdventOfCode.Api.Services;

public interface IInputProvider
{
    Task<IEnumerable<string>> GetPuzzleInput(int year, int day);
}
