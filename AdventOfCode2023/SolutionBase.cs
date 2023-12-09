using System.Reflection;

namespace AdventOfCode2023;

public abstract class SolutionBase
{
    protected abstract string PartOneInputFile { get; init; }
    protected abstract string PartTwoInputFile { get; init; }

    public abstract long PartOne();

    public abstract long PartTwo();

    protected virtual IEnumerable<string> GetFileContents(string filename, bool includeEmptyLines = false)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith(filename));

        using var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"\"{filename}\" not found."));

        foreach (var line in reader.ReadToEnd().Split("\r\n"))
        {
            if (string.IsNullOrEmpty(line) && !includeEmptyLines)
                continue;

            yield return line;
        }
    }
}
