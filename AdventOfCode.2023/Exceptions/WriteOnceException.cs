namespace AdventOfCode.Y2023.Exceptions;

public class WriteOnceException : Exception
{
    public WriteOnceException(string message, Exception innerException = null) : base(message, innerException) { }
}