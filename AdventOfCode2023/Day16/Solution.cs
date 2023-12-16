using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2023.Extensions;
using AdventOfCode2023.Models;

namespace AdventOfCode2023.Day16;
public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "16.txt";
    protected override string SampleInputOne { get; set; } = "16_sample.txt";

    public override long PartOne()
    {
        var data = GetFileContents(SolutionInput)
            .Select(l => l.Select(c => new Tile(c)).ToArray())
            .ToArray();

        var matrix = new Matrix<Tile>(data);

        var start = new Path(new Position(0, 0), Direction.East);
        var steps = 0;
        var queue = new Queue<Path>();
        queue.Enqueue(start);

        while (queue.Any())
        {
            steps++;
            var path = queue.Dequeue();
            var tile = matrix[path.Position];

            // update current tile if necessary
            if (tile.HasDirectionalBeam(path.Direction))
                continue;

            tile.SetDirectionalBeam(path.Direction);

            var nextPaths = path.NextDirections(tile.Value)
                .Select(d => new Path(path.Position.Next(d), d))
                .Where(p => matrix.CheckBounds(p.Position));

            foreach (var nextPath in nextPaths)
            {
                queue.Enqueue(nextPath);
            }
        }

        var result = matrix.Rows
            .SelectMany(r => r.Select(c => c.Energized ? 1 : 0)).Sum();

        Console.WriteLine($"\tSteps: {steps}");

        return result;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(SolutionInput)
            .Select(l => l.Select(c => new Tile(c)).ToArray());

        var srcMatrix = data.ToArray();

        var width = srcMatrix[0].Length;
        var height = srcMatrix.Length;

        var topStarts = Enumerable.Range(0, width).Select(x => new Path(new Position(x, 0), Direction.South));
        var leftStarts = Enumerable.Range(0, height).Select(y => new Path(new Position(0, y), Direction.East));
        var bottomStarts = Enumerable.Range(0, width).Select(x => new Path(new Position(x, height - 1), Direction.North));
        var rightStarts = Enumerable.Range(0, height).Select(y => new Path(new Position(width - 1, y), Direction.West));

        var greatestResult = 0;
        var semaphore = new SemaphoreSlim(1);

        var possibleStarts = topStarts.Concat(leftStarts).Concat(rightStarts).Concat(bottomStarts);

        var partitioner = Partitioner.Create(possibleStarts.ToList() , true);

        Parallel.ForEach(partitioner, new ParallelOptions { MaxDegreeOfParallelism = 16 }, (start, _) =>
        {
            var matrix = data.ToArray();
            var queue = new Queue<Path>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                steps++;
                var path = queue.Dequeue();
                var tile = matrix[path.Position.Y][path.Position.X];

                // update current tile if necessary
                if (tile.HasDirectionalBeam(path.Direction))
                    continue;

                tile.SetDirectionalBeam(path.Direction);

                var nextPaths = path.NextDirections(tile.Value)
                    .Select(d => new Path(path.Position.Next(d), d))
                    .Where(p => p.Position.X >= 0 && p.Position.X < width && p.Position.Y >= 0 && p.Position.Y < height);

                foreach (var nextPath in nextPaths)
                {
                    queue.Enqueue(nextPath);
                }
            }

            var result = matrix
                .SelectMany(r => r.Select(c => c.Energized ? 1 : 0)).Sum();

            semaphore.Wait();
            if (greatestResult < result)
                greatestResult = result;
            semaphore.Release();
        });

        return greatestResult;
    }
}

public class Path(Position position, Direction direction)
{
    public Position Position { get; init; } = position;
    public Direction Direction { get; init; } = direction;

    public IEnumerable<Direction> NextDirections(char c)
    {
        switch(c)
        {
            case '.':
                yield return Direction;

                break;
            case '-':
                if (Direction == Direction.West || Direction == Direction.East)
                    yield return Direction;
                else if (Direction == Direction.South || Direction == Direction.North)
                {
                    yield return Direction.West;
                    yield return Direction.East;
                }

                break;
            case '|':
                if (Direction == Direction.North || Direction == Direction.South)
                    yield return Direction;
                else if (Direction == Direction.West || Direction == Direction.East)
                {
                    yield return Direction.North;
                    yield return Direction.South;
                }

                break;
            case '/':
                yield return Direction switch
                {
                    Direction.North => Direction.East,
                    Direction.East => Direction.North,
                    Direction.West => Direction.South,
                    Direction.South => Direction.West,
                    _ => throw new ArgumentException("Invalid direction.")
                };

                break;
            case '\\':
                yield return Direction switch
                {
                    Direction.North => Direction.West,
                    Direction.West => Direction.North,
                    Direction.South => Direction.East,
                    Direction.East => Direction.South,
                    _ => throw new ArgumentException("Invalid direction.")
                };

                break;
        }
    }

    public override string ToString()
    {
        var directionChar = Direction switch
        {
            Direction.North => '^',
            Direction.West => '<',
            Direction.South => 'v',
            Direction.East => '>'
        };

        return $"{Position} {directionChar}";
    }
}

public class Tile(char value)
{
    private Direction _directionsMask;
    public bool Energized => _directionsMask > 0;
    public char Value { get; set; } = value;

    public bool HasDirectionalBeam(Direction direction) => _directionsMask.HasFlag(direction);

    public void SetDirectionalBeam(Direction direction)
    {
        if (Value == '-' && (direction == Direction.North || direction == Direction.South))
        {
            _directionsMask |= Direction.North;
            _directionsMask |= Direction.South;
        }
        else if (Value == '|' && (direction == Direction.West || direction == Direction.East))
        {
            _directionsMask |= Direction.West;
            _directionsMask |= Direction.East;
        }
        else
        {
            _directionsMask |= direction;
        }
    }
}