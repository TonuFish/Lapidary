namespace Lapidary.GemBuilder.Definitions;

#pragma warning disable CA1051 // Do not declare visible instance fields
internal unsafe ref struct GciTsObjInfo
{
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
	/// </list>
	/// </summary>
	internal uint access;

	internal ushort objectSecurityPolicyId;
	internal BitsMask _bits;

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

	internal enum BitsMask : ushort
	{
		implem_mask = 0x03,
		indexable_mask = 0x04,
		invariant_mask = 0x08,
		partial_mask = 0x10,
		overlay_mask = 0x20,

		/// <summary>
		/// Object is place holder for unsatisfied forward reference.
		/// </summary>
		is_placeholder = 0x40,

		swiz_kind_mask = 0x300,
#pragma warning disable CA1069 // Enums values should not be duplicated
		swiz_kind_shift = 8,
#pragma warning restore CA1069 // Enums values should not be duplicated
	}
}
#pragma warning restore CA1051 // Do not declare visible instance fields