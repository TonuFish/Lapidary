using Lapidary.Converters;

namespace Lapidary;

public interface ILapidaryManagementService
{
    public void ConfigureDatabase(
        DatabaseIdentifier identifier,
        DatabaseConnectionCredentials connection,
        IList<ILapidaryConverter>? converters = null);

    public EncryptedLoginCredentials EncryptCredentials(BasicLoginCredentials loginBucket);
    public SessionIdentifier Login(DatabaseIdentifier identifier, BasicLoginCredentials credentials);
    public SessionIdentifier LoginEncrypted(DatabaseIdentifier identifier, EncryptedLoginCredentials credentials);
    public void Logout(SessionIdentifier sessionIdentifier);

    public static string GetGemBuilderVersion()
    {
        Span<byte> buffer = stackalloc byte[128];
        buffer.Clear();
        return FFI.GetGemBuilderVersion(buffer).DecodeUTF8();
    }
}
