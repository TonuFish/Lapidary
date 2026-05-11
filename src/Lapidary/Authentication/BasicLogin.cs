namespace Lapidary.Authentication;

public sealed class BasicLogin : ILogin
{
	public LoginIdentifier Identifier { get; }

	internal bool IsEncrypted { get; }
	internal string Password { get; }
	internal string Username { get; }

	public BasicLogin(LoginIdentifier identifier, string username, string password, bool encrypted = true)
	{
		Identifier = identifier;
		IsEncrypted = encrypted;
		Password = password;
		Username = username;
	}
}
