using AdventOfCode.Api.Services;
using AdventOfCode.Y2023.Extensions;
using AdventOfCode.Y2023.Models;
using Microsoft.Extensions.Logging;

namespace AdventOfCode.Y2023.Day21;

public class Solution(IInputProvider inputProvider, ILogger logger) : Y2023Puzzle(inputProvider)
{
    protected ILogger _logger = logger;
    protected override int _day { get; init; } = 21;

    protected override long PartOne(IList<string> input)
    {
        var firstLine = input[0];
        var firstSpace = firstLine.IndexOf(' ');
        var steps = int.Parse(firstLine[..firstSpace]);
        var start = Position.Parse(firstLine[(firstSpace + 1)..]);

        var matrix = new Matrix<char>(input.Skip(1).Select(r => r.ToCharArray()).ToArray());

        var hashSets = new[] { new HashSet<Position>(4000), new HashSet<Position>(4000) };
        hashSets[0].Add(start);

        for (var i = 0; i < steps; i++)
        {
            var positions = hashSets[i % 2];
            Console.WriteLine($"i({i}): {positions.Count()}");

            foreach (var position in positions)
            {
                var nextPositions = hashSets[(i + 1) % 2];
                //nextPositions.Clear();

                matrix[position] = '.';
                var nextNeighbors = hashSets[i % 2].SelectMany(p => matrix.GetAdjacentValues(p, true)
                    .Where(v => !nextPositions.Contains(v) && v.Value != '#'));

                foreach (var neighbor in nextNeighbors)
                {
                    matrix[neighbor] = 'O';
                    nextPositions.Add(neighbor);
                }
            }
        }

        var result = hashSets[steps % 2].Count();

        return result;
    }

    protected override long PartTwo(IList<string> input)
    {
        var firstLine = input[0];
        var firstSpace = firstLine.IndexOf(' ');
        var start = Position.Parse(firstLine[(firstSpace + 1)..]);

        var matrix = new Matrix<char>(input.Skip(1).Select(r => r.ToCharArray()).ToArray());
        var gridSize = input[1].Length;

        const int goal = 26501365;
        var grids = goal / gridSize;
        var remainder = goal % gridSize;

        // There's some quadratic stuff that a people a lot smarter than I am noticed.
        // This code is pretty much copying their solution and trying to understand
        // how it works.

        var sequence = new List<int>();
        var work = new HashSet<Position> { start };
        var steps = 0;

        for (var n = 0; n < 3; n++)
        {
            for (; steps < n * gridSize + remainder; steps++)
            {
                // Another comment to point out my shameless copying.
                // Funky modulo arithmetic bc modulo of a negative number is negative, which isn't what we want here
                // (me): Generating positions to check based off the direction array, I don't understand the math that
                // is going on here. d.X % 131 + 131 % 131 seems like the last two operands are redundant?
                // e.g., 500 % 131 => 107, 107 + 131 + 238 => 238 % 131 = 107. Why can't we just mod 131 the coordinate components?
                //work = new HashSet<Position>(work
                //    .SelectMany(i => new[] { Direction.North, Direction.South, Direction.West, Direction.East }.Select(d => i.Next(d)))
                //    .Where(d => input[((d.X % 131) + 131) % 131][((d.Y % 131) + 131) % 131] != '#'));

                var directions = new[] { Direction.North, Direction.West, Direction.South, Direction.East };
                work = new HashSet<Position>(work
                    .SelectMany(i => directions.Select(d => i.Next(d))
                    .Where(p =>
                    {
                        // This is important for some reason; without it you get out of bounds indices...
                        var x = ((p.X % 131) + 131) % 131;
                        var y = ((p.Y % 131) + 131) % 131;

                        return matrix[x, y] != '#';
                    })));
                //.Where(d => input[((d.X % 131) + 131) % 131][((d.Y % 131) + 131) % 131] != '#'));

            }

            sequence.Add(work.Count);
        }

        // solve for the quadratic coefficients (I should have paid more attention in high school...)
        var c = sequence[0];
        var aPlusB = sequence[1] - c;
        var fourAPlusTwoB = sequence[2] - c;
        var twoA = fourAPlusTwoB - (2 * aPlusB);
        var a = twoA / 2;
        var b = aPlusB - a;

        long F(long n)
        {
            return a * (n * n) + b * n + c;
        }

        for (var i = 0; i < sequence.Count; i++)
        {
            Console.WriteLine($"{sequence[i]} : {F(i)}");
        }

        var result = F(grids);
        Console.WriteLine(F(grids));
        return result;
    }
}
