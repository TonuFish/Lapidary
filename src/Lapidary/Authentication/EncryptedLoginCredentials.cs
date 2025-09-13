namespace Lapidary;

public sealed class EncryptedLoginCredentials
{
    public required ReadOnlyMemory<byte> Password { get; init; }
    public required ReadOnlyMemory<byte> Username { get; init; }

    [SetsRequiredMembers]
    public EncryptedLoginCredentials(ReadOnlyMemory<byte> username, ReadOnlyMemory<byte> password)
    {
        Password = password;
        Username = username;
    }
}
