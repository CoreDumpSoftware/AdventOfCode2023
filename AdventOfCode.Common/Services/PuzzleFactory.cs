using System.Reflection;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Api.Services;

public interface IPuzzleFactory
{
    public Puzzle GetPuzzle(int year, int day);
}

public class PuzzleFactory(IInputProvider inputProvider, ILoggerFactory loggerFactory) : IPuzzleFactory
{
    private readonly IInputProvider _inputProvider = inputProvider;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public Puzzle GetPuzzle(int year, int day)
    {
        var dayString = day >= 10 ? day.ToString() : $"0{day}";
        var assembly = Assembly.Load($"AdventOfCode.{year}");
        var fullName = $"AdventOfCode.Y{year}.Day{dayString}.Solution";

        var type = assembly.GetType(fullName);
        if (type == null)
            throw new ArgumentException($"Unable to load puzzle solution for year {2023} day {19}.");

        var logger = _loggerFactory.CreateLogger(type);

        var puzzle = (Puzzle)Activator.CreateInstance(type, new object[] { _inputProvider, logger });
        if (puzzle == null)
            throw new Exception($"Unable to instance type: {type.FullName}");

        return puzzle;
    }
}
