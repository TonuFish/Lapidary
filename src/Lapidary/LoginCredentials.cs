namespace Lapidary;

public sealed class LoginCredentials
{
    public required string Password { get; init; }
    public required string Username { get; init; }

    [SetsRequiredMembers]
    public LoginCredentials(string username, string password)
    {
        Password = password;
        Username = username;
    }
}
