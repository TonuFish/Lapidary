namespace Lapidary.GemBuilder.Definitions;

#pragma warning disable CA1051 // Do not declare visible instance fields
/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcits.ht
/// </remarks>
public ref struct GciTsObjInfo
{
	public readonly bool IsIndexable => _bits == BitsMask.indexable_mask;
	public readonly bool IsInvariant => _bits == BitsMask.invariant_mask;
	public readonly bool IsOverlayed => _bits == BitsMask.overlay_mask;
	public readonly bool IsPartial => _bits == BitsMask.partial_mask;

	public OopType objId;

	/// <summary>
	/// OOP of the class of the obj.
	/// </summary>
	public OopType objClass;

	/// <summary>
	/// Obj's total size, in bytes or OOPs.
	/// </summary>
	public int64 objSize;

	/// <summary>
	/// Num of named inst vars in the obj.
	/// </summary>
	public int namedSize;

	/// <summary>
	/// <list type="bullet">
	/// <item>0 no auth</item>
	/// <item>1 read allowed</item>
	/// <item>2 write allowed</item>
	/// </list>
	/// </summary>
	public uint access;

	public ushort objectSecurityPolicyId;
	public BitsMask _bits;

	public GciTsObjInfo()
	{
		objId = ReservedOops.OOP_NIL;
		objClass = ReservedOops.OOP_NIL;
		objSize = 0;
		namedSize = 0;
		access = 0;
		objectSecurityPolicyId = 0;
		_bits = 0;
	}

	public readonly GciByteSwizEType ByteSwizKind()
	{
		return (GciByteSwizEType)((ushort)(_bits & BitsMask.swiz_kind_mask) >> 8);
	}

	public readonly byte ObjImpl()
	{
		return (byte)((ushort)_bits & 3); // GC_IMPLEMENTATION_MASK
	}
}
