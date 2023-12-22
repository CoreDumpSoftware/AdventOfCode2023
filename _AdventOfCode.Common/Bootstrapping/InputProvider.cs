using System.Text.Json;
using AdventOfCode.BoilerPlate.Configuration;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Bootstrapping;

public interface IInputProvider
{
    Task<IEnumerable<string>> GetPuzzleInput(int year, int day);
}

public class CachingInputProvider : IInputProvider
{
    private readonly string _session;
    private readonly IHttpClientFactory _httpClientFactory;

    public CachingInputProvider(IHttpClientFactory httpClientFactory, IOptions<Session> options)
    {
        _session = options.Value.Session;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<string>> GetPuzzleInput(int year, int day)
    {
        var cached = GetCachedInput(year, day);
        if (cached.Any())
            return cached;

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookies", _session);

        var response = await client.GetAsync(@"https://adventofcode.com/{year}/day/{day}");
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to retrieve input for {year} day {19}.");
        }

        var lines = JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
        SaveInput(year, day, lines);

        return lines;
    }

    private IEnumerable<string> GetCachedInput(int year, int day)
    {
        var filename = GetFileName(year, day);
        if (File.Exists(filename))
        {
            return File.ReadAllLines(filename);
        }
        else
        {
            return Enumerable.Empty<string>();
        }
    }

    private void SaveInput(int year, int day, IEnumerable<string> lines)
    {
        File.WriteAllLines(GetFileName(year, day), lines);
    }

    private string GetFileName(int year, int day) => $"{year}-day-{day}.txt";

}
