namespace AdventOfCode2023.Utilties;

public class WriteOnceException : Exception
{
    public WriteOnceException(string message,  Exception innerException = null) : base(message, innerException) { }
}