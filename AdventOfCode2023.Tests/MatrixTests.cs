using AdventOfCode2023.Models;

namespace AdventOfCode2023.Tests;

public class MatrixTests
{
    [Theory]
    [InlineData("(0, 0)", "(1, 0)", "(0, 1)", "(1, 1)")] // Upper left corner
    [InlineData("(1, 1)", "(0, 0)", "(1, 0)", "(2, 0)", "(0, 1)", "(2, 1)", "(0, 2)", "(1, 2)", "(2, 2)")] // In middle of matrix
    public void TestGetAdjacentValuesWithCorners(string target, params string[] positionStrings)
    {
        var matrix = new Matrix<int>(3);

        var adjacentValues = matrix.GetAdjacentValues(target, false).ToArray();

        var expectedPositions = positionStrings.Select(Position.Parse).ToArray();

        adjacentValues.Length.Should().Be(expectedPositions.Length);
        positionStrings.Select(Position.Parse).Should().BeEquivalentTo(adjacentValues.Cast<Position>());
    }

    [Theory]
    [InlineData("(0, 0)", "(1, 0)", "(0, 1)")] // Upper left corner
    [InlineData("(1, 1)", "(1, 0)", "(0, 1)", "(2, 1)", "(1, 2)")] // In middle of matrix
    public void TestGetAdjacentValuesWithoutCorners(string target, params string[] positionStrings)
    {
        var matrix = new Matrix<int>(3);

        var adjacentValues = matrix.GetAdjacentValues(target, true).ToArray();

        var expectedPositions = positionStrings.Select(Position.Parse).ToArray();

        adjacentValues.Length.Should().Be(expectedPositions.Length);
        positionStrings.Select(Position.Parse).Should().BeEquivalentTo(adjacentValues.Cast<Position>());
    }
}
