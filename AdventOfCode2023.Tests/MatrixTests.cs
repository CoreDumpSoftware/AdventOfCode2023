using AdventOfCode2023.Extensions;
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

public class IntExtensionTests
{
    [Theory]
    [InlineData(7, 3, 2)]
    [InlineData(4, 2, 1, 2)]
    public void TestGetBitDifferences(int left, int right, params int[] expectedIndices)
    {
        var differenceIndicies = left.GetBitDifferences(right).ToArray();

        differenceIndicies.Count().Should().Be(expectedIndices.Count());
        differenceIndicies.Should().BeEquivalentTo(expectedIndices);
    }
}