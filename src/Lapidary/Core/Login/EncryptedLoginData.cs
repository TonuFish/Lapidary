namespace Lapidary.Core.Login;

internal sealed class EncryptedLoginData : LoginData
{
	internal required ReadOnlyMemory<byte> Password { get; init; }
	internal required ReadOnlyMemory<byte> Username { get; init; }
}
