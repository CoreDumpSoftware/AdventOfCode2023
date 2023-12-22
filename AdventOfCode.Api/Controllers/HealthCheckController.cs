using Microsoft.AspNetCore.Mvc;

namespace AdventOfCode.Api.Controllers;

public class HealthCheckController(ILogger<HealthCheckController> logger) : Controller
{
    private readonly ILogger<HealthCheckController> _logger = logger;

    [HttpGet("/healthcheck")]
    public async Task<IActionResult> GetHealthCheck()
    {
        _logger.LogInformation($"Healthy!");

        return await Task.FromResult(new OkObjectResult("Healthy!"));
    }
}
