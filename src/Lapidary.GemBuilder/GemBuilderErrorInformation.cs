namespace Lapidary.GemBuilder;

/// <summary>
/// Storage for information contained in a <see cref="GciErrSType"/> and any worthwhile supplementary information.
/// </summary>
/// <param name="ArgCount"><see cref="GciErrSType.argCount"/></param>
/// <param name="Args"><see cref="GciErrSType.args"/></param>
/// <param name="Category"><see cref="GciErrSType.category"/></param>
/// <param name="Context"><see cref="GciErrSType.context"/></param>
/// <param name="ExceptionObj"><see cref="GciErrSType.exceptionObj"/></param>
/// <param name="Fatal"><see cref="GciErrSType.fatal"/></param>
/// <param name="Message"><see cref="GciErrSType.message"/></param>
/// <param name="Number"><see cref="GciErrSType.number"/></param>
/// <param name="Reason"><see cref="GciErrSType.reason"/></param>
/// <param name="Source">The <see cref="FFI"/> method call which generated this error.</param>
/// <param name="WhenUtc">The UTC time this error was logged.</param>
public sealed class GemBuilderErrorInformation
{
	// TODO: Consider visibility
	// TODO: Don't just mirror the underlying GS object here

	public required int ArgCount { get; init; }
	public required OopType[]? Args { get; init; }
	public required OopType Category { get; init; }
	public required OopType Context { get; init; }
	public required OopType ExceptionObj { get; init; }
	public required byte Fatal { get; init; }
	public required string? Message { get; init; }
	public required int Number { get; init; }
	public required string? Reason { get; init; }
	public required string Source { get; init; }
	public required DateTime WhenUtc { get; init; }
}
