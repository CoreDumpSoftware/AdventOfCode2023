using AdventOfCode2023.Day12;

namespace AdventOfCode2023.Tests.Day12;

public class RowPermutationsTests
{
    [Theory]
    [InlineData("???.### 1,1,3", "#.#.###")]
    [InlineData(".??..??...?##. 1,1,3",
        ".#...#....###.",
        ".#....#...###.",
        "..#..#....###.",
        "..#...#...###."
    )]
    [InlineData("?###???????? 3,2,1",
        ".###.##.#...",
        ".###.##..#..",
        ".###.##...#.",
        ".###.##....#",
        ".###..##.#..",
        ".###..##..#.",
        ".###..##...#",
        ".###...##.#.",
        ".###...##..#",
        ".###....##.#"
    )]
    [InlineData("????.######..#####. 1,6,5",
        "#....######..#####.",
	    ".#...######..#####.",
	    "..#..######..#####.",
	    "...#.######..#####."
    )]
    [InlineData("?.??.??? 1,2,3", "#.##.###")]
    [InlineData("??.??.??? 1,2,3", "#..##.###", ".#.##.###")]
    public void TestScanRow(string line, params string[] expectedPermutations)
    {
        var parts = line.Split(' ');
        var groups = parts[1].Split(',').Select(int.Parse).ToArray();

        var calculator = new RowPermutations(parts[0], groups);
        var expectedPermutationsList = expectedPermutations.ToList();

        var index = 0;
        foreach (var permutation in calculator)
        {
            expectedPermutationsList.Should().Contain(permutation);
            expectedPermutationsList.Remove(permutation);
        }

        expectedPermutationsList.Should().BeEmpty();
    }
}
