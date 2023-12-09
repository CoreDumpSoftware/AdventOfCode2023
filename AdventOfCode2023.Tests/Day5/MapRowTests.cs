using System.Collections;
using AdventOfCode2023.Day5;

namespace AdventOfCode2023.Tests.Day5;

public class MapRowTests
{
    public record TestData(string Line, long SourceRangeStart, long DestinationRangeStart, long Length, long[] SourceValues, long[] DestinationValues);

    public sealed class MapRowDataSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new[] { (object)new TestData("50 98 2", 98, 50, 2, [98, 99], [50, 51]) };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    [Theory]
    [ClassData(typeof(MapRowDataSource))]
    public void Test(TestData testData)
    {
        var map = MapRow.Parse(testData.Line);

        map.Source.Start.Should().Be(testData.SourceRangeStart);
        map.Destination.Start.Should().Be(testData.DestinationRangeStart);
        map.RangeLength.Should().Be(testData.Length);
    }
}
