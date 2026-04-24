namespace Lapidary.Core.Login;

internal sealed class BasicLoginData : LoginData
{
	internal required ReadOnlyMemory<byte> Password { get; init; }
	internal required ReadOnlyMemory<byte> Username { get; init; }
}
