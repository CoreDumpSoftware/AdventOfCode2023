namespace AdventOfCode.Y2023.Day08;

public class Node(string line)
{
    public static implicit operator string(Node n) => n.Value;

    public string Value { get; init; } = line[0..3];

    public string Left { get; init; } = line[7..10];
    public Node? LeftNode { get; set; }

    public string Right { get; init; } = line[12..15];
    public Node? RightNode { get; set; }
}
