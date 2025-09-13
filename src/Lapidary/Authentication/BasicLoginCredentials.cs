namespace Lapidary;

public sealed class BasicLoginCredentials
{
    public required string Password { get; init; }
    public required string Username { get; init; }

    [SetsRequiredMembers]
    public BasicLoginCredentials(string username, string password)
    {
        Password = password;
        Username = username;
    }
}
