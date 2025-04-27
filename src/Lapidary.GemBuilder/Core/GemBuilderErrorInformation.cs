namespace Lapidary.GemBuilder.Core;

/// <summary>
/// Storage for information contained in a <see cref="GciErrSType"/> and any worthwhile supplementary information.
/// </summary>
/// <param name="WhenUtc">The UTC time this error was logged.</param>
/// <param name="Source">The <see cref="FFI"/> method call which generated this error.</param>
/// <param name="Category"><see cref="GciErrSType.category"/></param>
/// <param name="Context"><see cref="GciErrSType.context"/></param>
/// <param name="ExceptionObj"><see cref="GciErrSType.exceptionObj"/></param>
/// <param name="Args"><see cref="GciErrSType.args"/></param>
/// <param name="Number"><see cref="GciErrSType.number"/></param>
/// <param name="ArgCount"><see cref="GciErrSType.argCount"/></param>
/// <param name="Fatal"><see cref="GciErrSType.fatal"/></param>
/// <param name="Message"><see cref="GciErrSType.message"/></param>
/// <param name="Reason"><see cref="GciErrSType.reason"/></param>
public sealed record GemBuilderErrorInformation(
    // TODO: This being public is temporary, it shouldn't be
    // TODO: Convert to class to avoid unnecessary record methods
    DateTime WhenUtc,
    string Source,
    OopType Category,
    OopType Context,
    OopType ExceptionObj,
    OopType[]? Args,
    int Number,
    int ArgCount,
    byte Fatal,
    string? Message,
    string? Reason);
