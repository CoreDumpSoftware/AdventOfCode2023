using AdventOfCode2023.Day7;

namespace AdventOfCode2023.Tests.Day7;

public class TestHand
{
    private static readonly List<char> StandardCardOrder = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' ];
    private static readonly List<char> JokerCardOrder = [ 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' ];

    [Theory]
    [InlineData("AAAAA 123", 123, HandType.FiveOfAKind, 'A')]
    [InlineData("AAAAK 123", 123, HandType.FourOfAKind, 'A', 'K')]
    [InlineData("AAKAA 123", 123, HandType.FourOfAKind, 'A', 'K')]
    [InlineData("AAAKK 123", 123, HandType.FullHouse, 'A', 'K')]
    [InlineData("AAKK2 123", 123, HandType.TwoPair, 'A', 'K', '2')]
    [InlineData("AAK23 123", 123, HandType.OnePair, 'A', 'K', '3', '2')]
    [InlineData("AK234 123", 123, HandType.HighCard, 'A', 'K', '4', '3', '2')]
    public void TestCreateWithStandardCardOrder(string line, int expectedBid, HandType expectedHandType, params char[] expectedCardTypesByGroup)
    {
        var hand = new Hand(line, StandardCardOrder);

        hand.Bid.Should().Be(expectedBid);
        hand.HandType.Should().Be(expectedHandType);
        hand.HandGroups.Select(h => h.Card).Should().BeEquivalentTo(expectedCardTypesByGroup);
    }

    public enum Comparison
    {
        Undefined = 0,
        LessThan,
        Equal,
        GreaterThan
    }

    [Theory]
    [InlineData("2345J", "2345A", Comparison.LessThan)]
    [InlineData("2345A", "J345A", Comparison.LessThan)]
    [InlineData("J345A", "32T3K", Comparison.LessThan)]
    [InlineData("32T3K", "Q2KJJ", Comparison.LessThan)]
    [InlineData("Q2KJJ", "T3T3J", Comparison.LessThan)]
    [InlineData("T3T3J", "KTJJT", Comparison.LessThan)]
    [InlineData("KTJJT", "KK677", Comparison.LessThan)]
    [InlineData("KK677", "T3Q33", Comparison.LessThan)]
    [InlineData("T3Q33", "T55J5", Comparison.LessThan)]
    [InlineData("T55J5", "QQQJA", Comparison.LessThan)]
    [InlineData("QQQJA", "Q2Q2Q", Comparison.LessThan)]
    [InlineData("Q2Q2Q", "2JJJJ", Comparison.LessThan)]
    [InlineData("2JJJJ", "2AAAA", Comparison.LessThan)]
    [InlineData("2AAAA", "JJJJ2", Comparison.LessThan)]
    [InlineData("JJJJ2", "JAAAA", Comparison.LessThan)]
    [InlineData("JAAAA", "AAAAJ", Comparison.LessThan)]
    [InlineData("AAAAJ", "JJJJJ", Comparison.LessThan)]
    [InlineData("JJJJJ", "AAAAA", Comparison.LessThan)]
    public void TestCompare(string leftHand, string rightHand, Comparison expectedComparison)
    {
        var left = new Hand(leftHand + " 0", StandardCardOrder);
        var right = new Hand(rightHand + " 0", StandardCardOrder);

        var comparer = new HandComparer(StandardCardOrder);

        var comparison = comparer.Compare(left, right) switch
        {
            0 => Comparison.Equal,
            -1 => Comparison.LessThan,
            1 => Comparison.GreaterThan
        };

        comparison.Should().Be(expectedComparison);
    }
}
