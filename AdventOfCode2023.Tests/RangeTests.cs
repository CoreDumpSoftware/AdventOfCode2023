using AdventOfCode2023.Models;

namespace AdventOfCode2023.Tests;

public class RangeTests
{
    [Theory]
    [InlineData(0, 2, 0, 1, 2)]
    [InlineData(-1, 1, -1, 0, 1)]
    public void TestEnumeration(int start, int end, params int[] expectedValues)
    {
        var range = new Range<int>(start, end);
        expectedValues.Should().BeEquivalentTo(range.ToArray());
    }

    [Theory]
    [InlineData(0, 5, 0, true)]
    [InlineData(0, 5, 5, true)]
    [InlineData(0, 5, -1, false)]
    [InlineData(0, 5, 6, false)]
    [InlineData(0, 139, 139, true)]
    public void TestContains(int start, int end, int value, bool expectedResult)
    {
        var range = new Range<int>(start, end);

        var result = range.Contains(value);

        result.Should().Be(expectedResult);
    }
}