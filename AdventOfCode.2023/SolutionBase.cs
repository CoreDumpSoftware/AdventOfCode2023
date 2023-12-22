using System.Reflection;

namespace AdventOfCode.Y2023;

public abstract class SolutionBase
{
    protected abstract string SolutionInput { get; init; }
    protected virtual string SampleInputOne { get; set; }
    protected virtual string SampleInputTwo { get; set; }
    protected virtual string SampleInputThree { get; set; }

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
