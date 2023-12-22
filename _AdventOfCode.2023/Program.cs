using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2023;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddUserSecrets(Assembly.GetAssembly(typeof(InputProvider));
        var config = builder.Build();

        if (args.Any())
        {
            RunAll();
        }
        else
        {
            var overallElapsed = new Stopwatch();
            overallElapsed.Start();

            var solution = new Day19.Solution();
            long answer = 0;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            answer = solution.PartOne();
            stopwatch.Stop();
            stopwatch.Restart();

            Console.WriteLine($"\tPart 1 Solution: {answer} \t\t {stopwatch.Elapsed}");

            stopwatch.Start();

            answer = solution.PartTwo();
            stopwatch.Stop();

            Console.WriteLine($"\tPart 2 Solution: {answer} \t\t {stopwatch.Elapsed}");

            overallElapsed.Stop();
            Console.WriteLine($"Finished running after {overallElapsed.Elapsed}");
        }
    }

    public static void RunAll()
    {
        foreach (var type in GetEnumerableOfType<SolutionBase>())
        {
            Console.WriteLine(type.GetType().FullName);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var answer = type.PartOne();
            Console.WriteLine($"\tPart 1 Solution: {answer} \t\t {stopwatch.Elapsed}");
            stopwatch.Stop();

            stopwatch.Reset();
            stopwatch.Start();
            answer = type.PartTwo();
            Console.WriteLine($"\tPart 2 Solution: {type.PartTwo()} {stopwatch.Elapsed}");
            stopwatch.Stop();

            Console.WriteLine();
        }
    }

    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
    {
        var baseType = typeof(T);

        var types = Assembly.GetAssembly(baseType).GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(baseType))
            .OrderBy(x => x.FullName);

        foreach (var type in types)
        {
            yield return (T)Activator.CreateInstance(type, constructorArgs);
        }
    }
}