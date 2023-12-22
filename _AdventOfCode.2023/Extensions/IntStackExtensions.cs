namespace AdventOfCode2023.Extensions;

public static class IntStackExtensions
{
    public static void IncrementTop(this Stack<int> stack)
    {
        stack.Push(stack.Pop() + 1);
    }

    public static void DecrementTop(this Stack<int> stack)
    {
        stack.Push(stack.Pop() - 1);
    }
}
