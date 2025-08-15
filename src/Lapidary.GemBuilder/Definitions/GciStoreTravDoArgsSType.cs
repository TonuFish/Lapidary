namespace Lapidary.GemBuilder.Definitions;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public unsafe struct GciStoreTravDoArgsSType
{
	// TODO: Scaffolded, absolutely not a proper implementation. Calculate manual offsets for the union.

	/// <summary>
	/// for GciStoreTravDoTravRefs and GciNbStoreTravDoTravRefs_ only
	///     0 == ExecuteStr, 1 == Perform 2 == ExecuteBlock
	///     3 == no execution , 4 == GciContinueWith .
	///     For no execution:
	///      if ((retrievalFlags & GCI_TRAV_REFS_EXCLUDE_RESULT_OBJ) == 0)
	///        use u.perform.receiver as the
	///        object to traverse after traversing altered objects,
	///      else ignore u.perform.receiver
	/// for all other Gci calls , 0 == ExecuteStr , all other values == Perform
	/// </summary>
	public int doPerform;

	public int doFlags;  /* per GciPerformNoDebug */
	public int alteredNumOops;   /* input/output */
	public BoolType alteredCompleted; /* output */

	// union `u`

	public GciTravBufType* storeTravBuff;
	public OopType* alteredTheOops;   /* output */
	public int storeTravFlags;   /* GCI_STORE_TRAV* bits from gci.ht */
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public unsafe struct GciPerformNoDebugArgs
{
	OopType receiver;
	// TODO: Manual offset instead of pad. Does the padding have the be zeroed?
	// char pad[24]; // Make later elements same offset as executestr, handy for GBS
	/* const */ byte* selector;  // 1 byte per character
	/* const */ OopType* args;
	int numArgs;
	ushort environmentId;  // compilation environment for execution
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public unsafe struct ExecuteStrOrBlockArgs
{
	public OopType contextObject;
	public OopType sourceClass; // String, Utf8 or Unicode7 or DoubleByteString
	public OopType symbolList;
	public int64 sourceSize;
	/* const */ public char* source; // 1 or 2 bytes per char, client-native byte order
	/* const */ public OopType* args;         // ignored unless ExecuteBlock
	public int numArgs;             // ignored unless ExecuteBlock
	public ushort environmentId;  // compilation environment for execution
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public struct GciContinueWithArgs
{
	public OopType process;
	public OopType replaceTopOfStack;
	// also uses doFlags above
	// GciErrSType *error input of GciContinueWith() always NULL
}
