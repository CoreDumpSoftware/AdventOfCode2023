using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Tests.Day1;

public class Day1LineSanitizerTests
{
    [Theory]
    [InlineData("123", "123")]
    [InlineData("one", "1")]
    [InlineData("two", "2")]
    [InlineData("three", "3")]
    [InlineData("four", "4")]
    [InlineData("five", "5")]
    [InlineData("six", "6")]
    [InlineData("seven", "7")]
    [InlineData("eight", "8")]
    [InlineData("nine", "9")]
    public void TestSanitize(string input, string expected)
    {
        var output = Day1LineSanitizer.SanitizeLine(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("oneight", "18")]
    [InlineData("oneightwo", "182")]
    [InlineData("oneightwone", "1821")]
    [InlineData("oneighthree", "183")]
    [InlineData("twone", "21")]
    [InlineData("twoneight", "218")]
    [InlineData("twoneighthree", "2183")]
    [InlineData("twoneighthreeight", "21838")]
    [InlineData("threeighthreeight", "3838")]
    [InlineData("fiveight", "58")]
    [InlineData("sevenine", "79")]
    [InlineData("sevenineight", "798")]
    public void TestCompoundedNumbersSanitize(string input, string expected)
    {
        var output = Day1LineSanitizer.SanitizeLine(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("two1nine", "219")]
    [InlineData("eightwothree", "823")]
    [InlineData("abcone2threexyz", "abc123xyz")]
    [InlineData("xtwone3four", "x2134")]
    [InlineData("4nineeightseven2", "49872")]
    [InlineData("zoneight234", "z18234")]
    [InlineData("7pqrstsixteen", "7pqrst6teen")]
    public void TestSampleInput(string input, string expected)
    {
        var output = Day1LineSanitizer.SanitizeLine(input);

        output.Should().Be(expected);
    }

    [Theory]
    [InlineData("449three45three", "4493453")]
    public void TestFileLines(string input, string expected)
    {
        var output = Day1LineSanitizer.SanitizeLine(input);

        output.Should().Be(expected);
    }
}