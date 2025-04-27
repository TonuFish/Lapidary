namespace Lapidary.GemBuilder.Definitions;

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

internal unsafe struct GciTravBufType
{
    internal uint allocatedBytes;
    internal uint usedBytes;
    internal fixed ByteType body[8];
}
