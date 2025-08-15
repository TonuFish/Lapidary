namespace Lapidary;

public sealed class DatabaseConnectionCredentials
{
    public required string GemService { get; init; }
    public string HostPassword { get; init; } = "";
    public string HostUserId { get; init; } = "";
    public required string StoneName { get; init; }

    [SetsRequiredMembers]
    public DatabaseConnectionCredentials(string gemService, string stoneName)
    {
        GemService = gemService;
        StoneName = stoneName;
    }
}