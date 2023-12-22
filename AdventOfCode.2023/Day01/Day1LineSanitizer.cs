using System.Text;

namespace AdventOfCode.Y2023.Day01;

public static class Day01LineSanitizer
{
    private enum Numbers
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }

    private record NumberValue(Numbers Number, int Value, string Text);

    private static readonly NumberValue[] NumberValues = Enum.GetValues<Numbers>()
        .Cast<Numbers>()
        .Select(n => new NumberValue(n, (int)n, n.ToString().ToLower()))
        .ToArray();

    private static readonly Dictionary<char, List<NumberValue>> NumberTextStartingChars =
        NumberValues.GroupBy(x => x.Text.First()).ToDictionary(g => g.Key, g => g.ToList());

    public static string SanitizeLine(string line)
    {
        var indexNumbers = new Dictionary<int, int>();
        var builder = new StringBuilder();

        for (var i = 0; i < line.Length; i++)
        {
            var somethingAdded = false;

            var group = NumberTextStartingChars.SingleOrDefault(g => g.Key == line[i]);
            if (group.Key == '\0')
            {
                builder.Append(line[i]);
                continue;
            }

            foreach (var numberValue in group.Value)
            {
                if (LookAheadForValue(line, numberValue.Text, i))
                {
                    var numberList = new List<int>();
                    var skipAhead = RecurseCompoundedNumbers(
                        line,
                        numberValue.Text,
                        i,
                        numberValue,
                        numberList
                    );

                    builder.Append(string.Join("", numberList));
                    somethingAdded = true;

                    i += skipAhead;
                }

                if (somethingAdded)
                    break;
            }

            if (!somethingAdded)
            {
                builder.Append(line[i]);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Looks ahead for the <paramref name="value"/> in the string at the current <paramref name="index"/>.
    /// </summary>
    /// <param name="line">The string to search.</param>
    /// <param name="value">The value to find.</param>
    /// <param name="index">The index to start.</param>
    /// <returns></returns>
    private static bool LookAheadForValue(string line, string value, int index)
    {
        if (line.Length < index + value.Length)
            return false;

        foreach (var c in value)
        {
            if (line[index++] != c)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Recursively finds "compounded" valid numbers such as oneight.
    /// </summary>
    /// <param name="line">The string to search.</param>
    /// <param name="value">The compounding value to find within the <paramref name="line"/>.</param>
    /// <param name="startIndex">The index to start looking for the <paramref name="value"/>.</param>
    /// <param name="currentNumber">The latest number added to the compounded number.</param>
    /// <param name="numberList">The list of numbers within the compounded number string.</param>
    /// <returns></returns>
    private static int RecurseCompoundedNumbers(
        string line,
        string value,
        int startIndex,
        NumberValue number,
        List<int> numberList)
    {
        var skipAhead = 0;
        var index = line.IndexOf(value, startIndex);
        var newValue = string.Empty;

        if (index >= startIndex)
        {
            skipAhead = number.Text.Length - 1;
            numberList.Add(number.Value);

            // Create "merged number"
            var lastChar = value.Last();
            var numbersStartingWithLastChar = NumberValues.Where(x => x.Text.StartsWith(lastChar));

            foreach (var numberItem in numbersStartingWithLastChar)
            {
                newValue = value[0..(value.Length - 1)];
                newValue += numberItem.Text;

                var charactersToBeReplaced = RecurseCompoundedNumbers(
                    line,
                    newValue,
                    startIndex,
                    numberItem,
                    numberList
                );

                if (charactersToBeReplaced > 0)
                {
                    skipAhead += charactersToBeReplaced;
                    break;
                }
            }
        }

        return skipAhead;
    }
}