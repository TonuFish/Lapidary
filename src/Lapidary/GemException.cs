namespace Lapidary;

// TODO: Unseal and specialise exceptions.
public sealed class GemException : Exception
{
    public GemException() : base()
    {
    }

    public GemException(string? message) : base(message)
    {
    }

    public GemException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
