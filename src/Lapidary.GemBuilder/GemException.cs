namespace Lapidary.GemBuilder;

// TODO: Unseal and specialise exceptions.
public sealed class GemException : Exception
{
	public required GemBuilderErrorInformation Error { get; init; }

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
