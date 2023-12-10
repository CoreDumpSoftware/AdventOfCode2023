namespace AdventOfCode2023.Day10;

public class DirectionException : Exception
{
    public DirectionException(string message, Exception innerException = null) : base(message, innerException) { }
}
