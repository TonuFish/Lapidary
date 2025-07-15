namespace Lapidary.GemBuilder.Definitions;
#pragma warning restore CA1051 // Do not declare visible instance fields

// TODO: Confirm if this should be marked as flags.

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
}