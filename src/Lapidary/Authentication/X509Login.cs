namespace Lapidary.Authentication;

public sealed class X509Login : ILogin
{
	// TODO
	public LoginIdentifier Identifier { get; }

	public X509Login(LoginIdentifier identifier)
	{
		Identifier = identifier;
	}
}
