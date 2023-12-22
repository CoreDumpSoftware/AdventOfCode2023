namespace AdventOfCode2023.Day4;

public class Solution : SolutionBase
{
    protected override string SolutionInput { get; init; } = "4_1.txt";

    public override long PartOne()
    {
        var data = GetFileContents(SolutionInput);
        var sum = 0;

        foreach (var line in data)
        {
            var winningNumbersStartIndex = line.IndexOf(':');
            var numbersStartIndex = line.IndexOf('|');

            var winningNumbers = ParseNumberString(line, winningNumbersStartIndex, numbersStartIndex);
            var numbers = ParseNumberString(line, numbersStartIndex);

            var intersect = numbers.Intersect(winningNumbers).ToArray();

            if (intersect.Length > 0)
            {
                var cardValue = (int)Math.Pow(2, intersect.Length - 1);
                sum += cardValue;
            }
        }

        return sum;
    }

    public override long PartTwo()
    {
        var data = GetFileContents(SolutionInput);
        var instanceCounts = new List<int>();

        var yIndex = 1;
        foreach (var line in data)
        {
            var winningNumbersStartIndex = line.IndexOf(':');
            var numbersStartIndex = line.IndexOf('|');

            var winningNumbers = ParseNumberString(line, winningNumbersStartIndex, numbersStartIndex);
            var numbers = ParseNumberString(line, numbersStartIndex);

            var intersect = numbers.Intersect(winningNumbers).ToArray();

            // Add a copy of the original cards to the instance counts.
            if (instanceCounts.Count < yIndex)
                instanceCounts.Add(1);
            else
                instanceCounts[yIndex - 1]++;

            // Add copies of cards based on winnings.
            if (intersect.Length > 0)
            {
                var currentCardCopies = instanceCounts[yIndex - 1]; // The total amount of instances for this card
                foreach (var cardNumber in Enumerable.Range(yIndex + 1, intersect.Length)) // Iterate over cards to add copies for
                {
                    // Add copies based on how many instances the current card has.
                    if (instanceCounts.Count < cardNumber) // If there's no list value for the current card, add one.
                        instanceCounts.Add(currentCardCopies);
                    else // else add on existing value
                        instanceCounts[cardNumber - 1] += currentCardCopies;
                }
            }

            yIndex++;
        }

        return instanceCounts.Sum();
    }

    /// <summary>
    /// Take a substring, parse the individual numbers into an array of ints.
    /// </summary>
    /// <param name="line">The line to get a substring from.</param>
    /// <param name="startIndex">The substring index to start getting numbers from.</param>
    /// <param name="length">The length of the substring to gather.</param>
    /// <returns>Array of numbers for the given substring.</returns>
    private static int[] ParseNumberString(string line, int startIndex, int length = -1) => line
            .Substring(startIndex + 1, (length == -1 ? line.Length : length) - startIndex - 1) // offset start to exclude separator chars
            .Trim()
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x)) // handles cases in strings where you have multiple spaces in between numbers
            .Select(int.Parse)
            .ToArray();
}

