using AdventOfCode2023.Models;
using Dist = System.ValueTuple<int, int>;

namespace AdventOfCode2023.Day17;

public class Dijkstra<TCell, TMid>
    where TCell : notnull
{
    /// <summary>
    /// Set this to provide lookup logic for neighboring cells
    /// </summary>
    public Func<TCell, IEnumerable<TMid>> GetNeighbors;

    /// <summary>
    /// Set this to retrieve the distance value for a cell.
    /// </summary>
    public Func<TMid, int> GetDistance;

    /// <summary>
    /// Set this provide a transformation on the neighboring cell. (Maybe?)
    /// </summary>
    public Func<TCell, TMid, TCell> GetCell;

    /// <summary>
    /// Setting a heuristic will cause the pathfinder to behave like A*
    /// </summary>
    public Func<TCell, int> Heuristic = _ => 0;

    /// <summary>
    /// Computes start against all possible inputs.
    /// </summary>
    /// <param name="start">The starting item.</param>
    /// <param name="all">The </param>
    /// <returns></returns>
    public Dictionary<TCell, int> ComputeAll(TCell start, IEnumerable<TCell> all)
    {
        var distances = new Dictionary<TCell, int>();

        // Set all distance values to the max value to so that any other positive
        // int32 value will override.
        foreach (var cell in all)
        {
            distances[cell] = int.MaxValue;
        }

        var queue = new PriorityQueue<TCell, int>(distances.Select(kvp => (kvp.Key, kvp.Value)));

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            var current = distances[cell];

            foreach (var neighbor in GetNeighbors(cell))
            {
                var other = GetCell(cell, neighbor);
                var weight = GetDistance(neighbor);

                if (current + weight < distances[other])
                {
                    distances[other] = current + weight;
                }
            }
        }

        return distances;
    }

    /// <summary>
    /// Calculates the path cost given an individual cell.
    /// </summary>
    /// <param name="start">The starting cell.</param>
    /// <param name="validateCellFn">An optional function parameter to skip neighboring cells on a specified condition.</param>
    /// <returns>Returns a dictionary containing distances to various cells from the start.</returns>
    public Dictionary<TCell, int> Compute(TCell start, Predicate<TCell> validateCellFn)
    {
        var distances = new DefaultDictionary<TCell, int> { DefaultValue = int.MaxValue, [start] = 0 };
        var seen = new HashSet<TCell>();
        var queue = new PriorityQueue<TCell, Dist>();

        queue.Enqueue(start, new Dist(Heuristic(start), 0));

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();

            if (seen.Contains(cell))
                continue;

            seen.Add(cell);

            var current = distances[cell];

            foreach (var neighbor in GetNeighbors(cell))
            {
                var other = GetCell(cell, neighbor);
                if (!validateCellFn(other))
                    continue;

                var weight = GetDistance(neighbor);
                var newScore = current + weight;

                if (!seen.Contains(other))
                    queue.Enqueue(other, new Dist(newScore + Heuristic(other), newScore))
                        ;
                if (newScore < distances[other])
                    distances[other] = newScore;
            }
        }
        return distances;
    }
}
