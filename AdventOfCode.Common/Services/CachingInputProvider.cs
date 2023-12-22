using AdventOfCode.Api.Configuration;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Api.Services;

public class CachingInputProvider : IInputProvider
{
    private static readonly string CacheDirectory = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), "AdventOfCode");

    private readonly string _session;
    private readonly IHttpClientFactory _httpClientFactory;

    public CachingInputProvider(IHttpClientFactory httpClientFactory, IOptions<UserSecrets> options)
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
        client.DefaultRequestHeaders.Add("Cookie", _session);

        var url = $"https://adventofcode.com/{year}/day/{day}/input";

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to retrieve input for {year} day {19}.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var lines = content.Split("\n");

        SaveInput(year, day, lines);

        return lines;
    }

    private IEnumerable<string> GetCachedInput(int year, int day)
    {
        if (!Directory.Exists(CacheDirectory))
            Directory.CreateDirectory(CacheDirectory);

        var yearFolder = Path.Combine(CacheDirectory, year.ToString());
        if (!Directory.Exists(yearFolder))
            Directory.CreateDirectory(yearFolder);

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
        var filePath = GetFileName(year, day);

        File.WriteAllLines(filePath, lines);
    }

    private string GetFileName(int year, int day) => Path.Combine(CacheDirectory, year.ToString(), $"Day{day}.txt");

}
