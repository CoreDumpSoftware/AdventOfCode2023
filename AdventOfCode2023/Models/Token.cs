namespace AdventOfCode2023.Models;

public struct Token(string value, char endingCharacter)
{
    public static implicit operator string(Token t) => t.Value;

    public string Value { get; set; } = value;
    public char EndingCharacter { get; set; } = endingCharacter;
}
