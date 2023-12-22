using AdventOfCode.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdventOfCode.Api.Controllers;

public class PuzzleController(IPuzzleFactory puzzleFactory, ILogger<PuzzleController> logger) : Controller
{
    private readonly IPuzzleFactory _puzzleFactory = puzzleFactory;
    private readonly ILogger<PuzzleController> _logger = logger;

    [HttpGet("/{year}/day/{day}/part/{part}")]
    public async Task<IActionResult> RunPuzzle(int year, int day, int part)
    {
        _logger.LogInformation($"Executing year {year}, day {day}, part {part} puzzle...");

        var puzzle = _puzzleFactory.GetPuzzle(year, day);
        var result = await puzzle.Solve(part);

        _logger.LogInformation(result.ToString());

        return Ok(result.ToString());
    }

    [HttpPost("/{year}/day/{day}/part/{part}")]
    public async Task<IActionResult> RunPuzzleWithSampleInput(int year, int day, int part)
    {
        _logger.LogInformation($"Executing year {year}, day {day}, part {part} puzzle...");

        var puzzle = _puzzleFactory.GetPuzzle(year, day);

        var input = await GetPostedInput();
        var result = await puzzle.Solve(part, input);

        _logger.LogInformation(result.ToString());

        return Ok(result.ToString());
    }

    //private async Task<IEnumerable<string>> GetPostedInput()
    private async Task<List<string>> GetPostedInput()
    {
        using var reader = new StreamReader(Request.Body);
        var line = await reader.ReadLineAsync();
        var input = new List<string>();

        //var line = reader.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            input.Add(line);
            line = await reader.ReadLineAsync();
        }

        return input;
    }
}
