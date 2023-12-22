namespace AdventOfCode.Y2023.Day18;

public static class FloodFill<TCell>
{
    public static int Fill(
        TCell start,
        Func<TCell, IEnumerable<TCell>> onGetNeighbors,
        Func<TCell, bool> onValidate,
        Action<TCell> onFill)
    {
        var visited = new HashSet<TCell>();
        var stack = new Stack<TCell>();

        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;

            visited.Add(current);
            onFill(current);

            foreach (var neighbor in onGetNeighbors(current).Where(onValidate))
            {
                if (visited.Contains(neighbor))
                    continue;

                stack.Push(neighbor);
            }
        }

        return visited.Count;
    }
}