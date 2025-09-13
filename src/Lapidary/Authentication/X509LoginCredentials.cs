namespace Lapidary;

public sealed class X509LoginCredentials
{
    // TODO: Readable property names.

    public required string NetldiHostOrIp { get; init; }
    public required string NetldiNameOrPort { get; init; }
    public required string PrivateKey { get; init; }
    public required string Cert { get; init; }
    public required string CaCert { get; init; }
    public required string ExtraGemArgs { get; init; }
    public required string DirArg { get; init; }
    public required string LogArg { get; init; }
    public required bool ArgsArePemStrings { get; init; }
}
