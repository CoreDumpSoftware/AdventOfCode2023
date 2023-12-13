using AdventOfCode2023.Extensions;

namespace AdventOfCode2023.Day12;

public class RowResolver(string line, int[] groups, bool debugPrinting = false)
{
    private readonly string _line = line;
    private readonly int[] _groups = groups;
    private readonly bool _debugPrinting = debugPrinting;

    public int ResolveArrangements()
    {
        return RecurseLine(_line, 0, new Stack<int>(_line.Length), false);
    }

    private int RecurseLine(string line, int lineIndex, Stack<int> setStack, bool buildingSet)
    {
        var result = 0;
        var isEOL = IsEndOfLine(lineIndex);
        var current = !isEOL ? line[lineIndex] : '\0';
        var nextIndex = lineIndex + 1;

        var addOrIncrement = (bool b) => { if (b) setStack.IncrementTop(); else setStack.Push(1); };
        var removeOrDecrement = (bool b) => { if (b) setStack.DecrementTop(); else setStack.Pop(); };

        switch (current)
        {
            case '\0':
                if (SetCountsMatchExpectedGroups(setStack))
                {
                    result++;

                    if (_debugPrinting)
                        Console.WriteLine(line);
                }

                break;
            case '.':
                result += RecurseLine(line, nextIndex, setStack, false);
                break;
            case '#':
                addOrIncrement(buildingSet);

                if (CurrentStackExceedsGroupValues(setStack))
                {
                    removeOrDecrement(buildingSet);
                    return result;
                }

                result += RecurseLine(line, nextIndex, setStack, true);

                removeOrDecrement(buildingSet);

                break;
            case '?':
                var newLine = line.ReplaceAt(lineIndex, 1, ".");
                result += RecurseLine(newLine, nextIndex, setStack, false);

                addOrIncrement(buildingSet);

                if (!CurrentStackExceedsGroupValues(setStack))
                {
                    removeOrDecrement(buildingSet);
                    return result;
                }

                newLine = line.ReplaceAt(lineIndex, 1, "#");
                result += RecurseLine(newLine, nextIndex, setStack, true);

                removeOrDecrement(buildingSet);

                break;
        }

        return result;
    }

    private bool IsEndOfLine(int index) => index >= _line.Length;

    private bool SetCountsMatchExpectedGroups(IEnumerable<int> setCounts) =>
        setCounts.Reverse().SequenceEqual(_groups);

    private bool CurrentStackExceedsGroupValues(Stack<int> stack)
    {
        var stackCount = stack.Count();
        var lastSet = stack.Last();

        if (stackCount > _groups.Length)
            return false;

        if (lastSet > _groups.Skip(stackCount - 1).First())
            return false;

        return true;
    }
}
