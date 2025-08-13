namespace Lapidary.GemBuilder.Definitions;

#pragma warning disable CA1051 // Do not declare visible instance fields
// TODO: Assuming memory layout of child here, confirm that at some point...

/// <summary>
/// 
/// </summary>
/// <remarks>
/// gcits.ht
/// </remarks>
internal ref struct GciTsGbjInfo
{
	#region Gbj Specific Functionality

	private enum ExtraBits : uint64
	{
		is_7bits = 0x1,  // from object header
		is_utf16 = 0x2,
		// is_utf8 not needed, instances of Utf8 have data returned to java as 7bit or as Utf16
		strInfo_ChrSize = 0x7,
		strInfo_icuString = 0x8, // Unicode7,Unicode16,Unicode32
		strInfo_byteArrBit = 0x10,  // a ByteArray without Utf8 nor Utf16 encoding
		strInfo_shift = 17,
		strSymInfo_isSymbol = 0x20,
	}

	internal readonly bool Is7Bits => (extraBits & (uint64)ExtraBits.is_7bits) > 0;
	internal readonly bool IsByteArray => ((extraBits >> 17) & (uint64)ExtraBits.strInfo_byteArrBit) > 0;
	internal readonly bool IsLargeInteger => (extraBits & (1UL << 34)) > 0;
	internal readonly bool IsNumber => (extraBits & (1UL << 23)) > 0;
	internal readonly bool IsScaledDecimal => (extraBits & (1UL << 31)) > 0;
	internal readonly bool IsUtf16 => (extraBits & (uint64)ExtraBits.is_utf16) > 0;
	internal readonly bool IsUnicode => ((extraBits >> 17) & (uint64)ExtraBits.strInfo_icuString) > 0;

	public uint64 CharSize()
	{
		return (extraBits >> 17) & (uint64)ExtraBits.strInfo_ChrSize;
	}

	#endregion Gbj Specific Functionality

	internal readonly bool IsIndexable => _bits == BitsMask.indexable_mask;
	internal readonly bool IsInvariant => _bits == BitsMask.invariant_mask;
	internal readonly bool IsOverlayed => _bits == BitsMask.overlay_mask;
	internal readonly bool IsPartial => _bits == BitsMask.partial_mask;

	internal OopType objId;

	/// <summary>
	/// OOP of the class of the obj.
	/// </summary>
	internal OopType objClass;

	/// <summary>
	/// Obj's total size, in bytes or OOPs.
	/// </summary>
	internal int64 objSize;

	/// <summary>
	/// Num of named inst vars in the obj.
	/// </summary>
	internal int namedSize;

	/// <summary>
	/// <list type="bullet">
	/// <item>0 no auth</item>
	/// <item>1 read allowed</item>
	/// <item>2 write allowed</item>
	/// <item>4 auth not exist</item>
	/// </list>
	/// </summary>
	internal uint access;

	internal ushort objectSecurityPolicyId;
	internal BitsMask _bits;

	internal uint64 extraBits;
	internal int64 bytesReturned;

	public GciTsGbjInfo()
	{
		objId = ReservedOops.OOP_NIL;
		objClass = ReservedOops.OOP_NIL;
		objSize = 0;
		namedSize = 0;
		access = 0;
		objectSecurityPolicyId = 0;
		_bits = 0;
		extraBits = 0;
		bytesReturned = 0;
	}

	internal readonly GciByteSwizEType ByteSwizKind()
	{
		return (GciByteSwizEType)((ushort)(_bits & BitsMask.swiz_kind_mask) >> 8);
	}

	internal readonly byte ObjImpl()
	{
		return (byte)((ushort)_bits & 3); // GC_IMPLEMENTATION_MASK
	}
}
#pragma warning restore CA1051 // Do not declare visible instance fields
