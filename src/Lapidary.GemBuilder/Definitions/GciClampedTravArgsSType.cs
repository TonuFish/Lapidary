namespace Lapidary.GemBuilder.Definitions;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcicmn.ht
/// </remarks>
internal unsafe struct GciClampedTravArgsSType
{
	internal OopType clampSpec;
	internal OopType resultOop; /* Result of GciPerformTrav/GciExecuteStrTrav */
	internal GciTravBufType* travBuff;
	internal int level;
	internal int retrievalFlags;
	internal BoolType isRpc; /* private, for use by implementation of GCI */

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
internal unsafe struct GciTravBufType
{
	// TODO: Revisit this around the size of body................

	internal uint allocatedBytes;
	internal uint usedBytes;
	// TODO: Migrate fixed arrays to InlineArray
	internal fixed ByteType body[8];
}
