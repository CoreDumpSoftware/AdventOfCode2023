namespace AdventOfCode.Y2023.Day07;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "7_1.txt";

    public override long PartOne()
    {
        var data = GetFileContents(SolutionInput);
        var hands = new List<Hand>();

        List<char> cardsPriority = [ '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' ];

        foreach (var line in data)
        {
            var hand = new Hand(line, cardsPriority);
            hands.Add(hand);
        }

        hands.Sort(new HandComparer(cardsPriority));

        long result = 0;
        long rankMultiplier = 1;
        foreach (var hand in hands)
        {
            result += (hand.Bid * rankMultiplier++);
        }

        return result;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(SolutionInput);
        var hands = new List<Hand>();

        List<char> cardsPriority = ['J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'];

        foreach (var line in data)
        {
            var hand = new WildHand(line, cardsPriority);
            hands.Add(hand);
        }

        hands.Sort(new HandComparer(cardsPriority));

        long result = 0;
        long rankMultiplier = 1;
        foreach (var hand in hands)
        {
            result += (hand.Bid * rankMultiplier++);
        }

        return result;
    }
}

public class HandComparer : IComparer<Hand>
{
    private readonly List<char> _cardPriority;

    public HandComparer(List<char> cardPriority)
    {
        _cardPriority = cardPriority;
    }

    public int Compare(Hand? x, Hand? y)
    {
        if (x == null || y == null)
            throw new ArgumentNullException();

        var handTypeDiff = (int)y.HandType - (int)x.HandType;

        if (x.HandType > y.HandType)
            return 1;
        else if (y.HandType > x.HandType)
            return -1;

        for (var i = 0; i < 5; i++)
        {
            var delta = _cardPriority.IndexOf(x.Cards[i]) - _cardPriority.IndexOf(y.Cards[i]);
            if (delta != 0)
                return delta > 0 ? 1 : -1;
        }

        return 0;
    }
}

public class WildHand : Hand
{
    public WildHand(string line, List<char> cardsPriority) : base(line, cardsPriority)
    {
    }

    private int HandTypeToInt(HandType type) => type switch
    {
        HandType.HighCard => 1,
        HandType.OnePair => 2,
        HandType.ThreeOfAKind => 3,
        HandType.FourOfAKind => 4,
        HandType.FiveOfAKind => 5
    };

    protected override HandType GetHandType()
    {
        var firstNonJackGroup = HandGroups.FirstOrDefault(g => g.Card != 'J');
        if (firstNonJackGroup == null) // If all cards are jacks then it's a five of a kind
            return HandType.FiveOfAKind;

        var jackGroup = HandGroups.FirstOrDefault(g => g.Card == 'J');

        if (jackGroup != null)
        {
            HandGroups.Remove(firstNonJackGroup);
            HandGroups.Remove(jackGroup);

            var jackCount = HandTypeToInt(jackGroup.HandType);
            var bestCount = HandTypeToInt(firstNonJackGroup.HandType);

            var newType = (jackCount + bestCount) switch
            {
                2 => HandType.OnePair,
                3 => HandType.ThreeOfAKind,
                4 => HandType.FourOfAKind,
                5 => HandType.FiveOfAKind
            };

            HandGroups.Insert(0, new(newType, firstNonJackGroup.Card));
        }

        return base.GetHandType();
    }
}

public class Hand
{
    public HandType HandType { get; init; }
    public List<HandGroup> HandGroups { get; init; }
    public string Cards { get; set; }
    public long Bid { get; set; }
    public List<char> CardsPriority { get; init; }

    public Hand(string line, List<char> cardsPriority)
    {
        CardsPriority = cardsPriority;

        var parts = line.Split(' ');
        var cards = parts[0].Trim();

        HandGroups = ParseHand(cards);
        HandType = GetHandType();

        Cards = parts[0].Trim();
        Bid = long.Parse(parts[1].Trim());
        //HandGroups = groups;
        //HandType = handType;
    }

    protected virtual List<HandGroup> ParseHand(string cards)
    {
        var dict = cards.GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        var groups = new List<HandGroup>();

        foreach (var kvp in dict)
        {
            switch (kvp.Value)
            {
                case 5:
                    groups.Add(new(HandType.FiveOfAKind, kvp.Key));
                    break;
                case 4:
                    groups.Add(new(HandType.FourOfAKind, kvp.Key));
                    break;
                case 3:
                    groups.Add(new(HandType.ThreeOfAKind, kvp.Key));
                    break;
                case 2:
                    groups.Add(new(HandType.OnePair, kvp.Key));
                    break;
                case 1:
                    groups.Add(new(HandType.HighCard, kvp.Key));
                    break;
            }
        }

        return groups
            .OrderByDescending(x => (int)x.HandType)
            .ThenBy(x => CardsPriority.IndexOf(x.Card))
            .ToList();
    }

    protected virtual HandType GetHandType()
    {
        return HandGroups.Count switch
        {
            1 => HandType.FiveOfAKind,
            2 => HandGroups[0].HandType == HandType.FourOfAKind
                ? HandType.FourOfAKind
                : HandType.FullHouse,
            3 => HandGroups[0].HandType == HandType.ThreeOfAKind
                ? HandType.ThreeOfAKind
                : HandType.TwoPair,
            4 => HandType.OnePair,
            _ => HandType.HighCard
        };
    }
}

public class HandGroup(HandType handType, char card)
{
    public HandType HandType { get; init; } = handType;
    public char Card { get; init; } = card;
}

public enum HandType
{
    HighCard = 1,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}