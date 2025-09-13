namespace Lapidary.GemBuilder.Definitions;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public unsafe struct GciClampedTravArgsSType
{
	public OopType clampSpec;
	public OopType resultOop; /* Result of GciPerformTrav/GciExecuteStrTrav */
	public GciTravBufType* travBuff;
	public int level;
	public int retrievalFlags;
	public BoolType isRpc; /* private, for use by implementation of GCI */

	public GciClampedTravArgsSType()
	{
		clampSpec = ReservedOops.OOP_NIL;
		resultOop = ReservedOops.OOP_NIL;
		travBuff = default;
		level = 0;
		retrievalFlags = 0;
		isRpc = 1;
	}
}

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
public unsafe struct GciTravBufType
{
	// TODO: Revisit this around the size of body................

	public uint allocatedBytes;
	public uint usedBytes;
	// TODO: Migrate fixed arrays to InlineArray
	public fixed ByteType body[8];
}
