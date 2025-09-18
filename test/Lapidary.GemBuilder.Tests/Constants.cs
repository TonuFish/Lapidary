namespace Lapidary.GemBuilder.Tests;

internal static class Constants
{
	internal static class Connection
	{
		internal static ReadOnlySpan<byte> GemService => "!tcp@localhost#netldi:50377#task!gemnetobject\0"u8;
		internal static ReadOnlySpan<byte> HostPassword => "\0"u8;
		internal static ReadOnlySpan<byte> HostUsername => "\0"u8;
		internal static ReadOnlySpan<byte> Password => "swordfish\0"u8;
		internal static ReadOnlySpan<byte> StoneName => "!@localhost!gs64stone\0"u8;
		internal static ReadOnlySpan<byte> Username => "DataCurator\0"u8;
	}
}
