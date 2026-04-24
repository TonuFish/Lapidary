namespace Lapidary.Authentication;

public sealed class BasicLogin : ILogin
{
	public LoginIdentifier Identifier { get; init; }

	internal bool IsEncrypted { get; init; }
	internal string Password { get; init; }
	internal string Username { get; init; }

	public BasicLogin(LoginIdentifier identifier, string username, string password, bool encrypted = true)
	{
		Identifier = identifier;
		IsEncrypted = encrypted;
		Password = password;
		Username = username;
	}
}
