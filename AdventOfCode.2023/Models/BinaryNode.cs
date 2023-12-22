namespace AdventOfCode.Y2023.Models;

public class BinaryNode<T>
{
    public string? Name { get; set; }
    public T Data { get; set; }
    public BinaryNode<T>? Parent { get; set; }
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public override string ToString() => Name;
}
