namespace Lapidary.GemBuilder.Definitions;

#pragma warning disable CA1051 // Do not declare visible instance fields
/// <summary>
/// GCI error message.
/// </summary>
/// <remarks>
/// gci.ht
/// </remarks>
internal unsafe ref struct GciErrSType
{
    /// <summary>
    /// Error dictionary.
    /// </summary>
    internal OopType category;

    /// <summary>
    /// GemStone Smalltalk execution state, a GsProcess.
    /// </summary>
    internal OopType context;

    /// <summary>
    /// An instance of AbstractException, or nil; may be nil if error was not signaled from Smalltalk execution.
    /// </summary>
    internal OopType exceptionObj;

    /// <summary>
    /// Arguments to error text.
    /// </summary>
    internal fixed OopType args[Constants.GCI_MAX_ERR_ARGS];

    /// <summary>
    /// GemStone error number.
    /// </summary>
    internal int number;

    /// <summary>
    /// Num of arg in the args[].
    /// </summary>
    internal int argCount;

    /// <summary>
    /// Nonzero if err is fatal.
    /// </summary>
    internal byte fatal;

    /// <summary>
    /// Null-terminated Utf8 error text.
    /// </summary>
    internal fixed byte message[Constants.GCI_ERR_STR_SIZE + 1];

    /// <summary>
    /// Null-terminated Utf8.
    /// </summary>
    internal fixed byte reason[Constants.GCI_ERR_reasonSize + 1];

    public GciErrSType()
    {
        // TODO: Migrate fixed arrays to InlineArray, make struct readonly

        category = ReservedOops.OOP_NIL;
        context = ReservedOops.OOP_NIL;
        exceptionObj = ReservedOops.OOP_NIL;

        fixed (OopType* argsBase = args)
        {
            *argsBase = ReservedOops.OOP_ILLEGAL;
        }

        number = 0;
        argCount = 0;
        fatal = 0;

        fixed (byte* messageBase = message)
        {
            *messageBase = 0;
        }

        fixed (byte* reasonBase = reason)
        {
            *reasonBase = 0;
        }
    }
}
#pragma warning restore CA1051 // Do not declare visible instance fields