using Lapidary.Authentication;

namespace Lapidary.Configuration.Validation;

internal sealed class GemStoneConfigurationBuilderValidator
{
	public void Validate<T>(GemStoneConfigurationBuilder<T> builder) where T : GemStone<T>
	{
		// TODO: Use T or drop it.
		// TODO: Proper validation?
		// TODO: Actual error processing.
		// TODO: Finalise validation.

		// GemService
		if (!ValidateGemStoneField(builder.GemService))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		//HostPassword
		if (!ValidateUnixField(builder.HostPassword))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		//HostUserId
		if (!ValidateUnixField(builder.HostUserId))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		//StoneName
		if (!ValidateGemStoneField(builder.StoneName))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		// Logins
		foreach (var (_, login) in builder.IdentifiersToLogins)
		{
			if (!ValidateLogin(login))
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}
		}

		//ValidatingIdentifier
		//ValidatingLogin
		if (!ValidateValidatingUser(builder.ValidatingLogin, builder.ValidatingIdentifier, builder.IdentifiersToLogins))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		//Converters
	}

	private bool ValidateGemStoneField(ReadOnlySpan<char> text)
	{
		return text.Length <= 100 && text.ContainsOnlyExtendedAscii();
	}

	private bool ValidateLogin(ILogin login)
	{
		return login switch
		{
			BasicLogin b => ValidateBasicLogin(b),
			X509Login x => ValidateX509Login(x),
			_ => ThrowHelper.ThrowUnreachableException<bool>(),
		};

		static bool ValidateBasicLogin(BasicLogin login)
		{
			return !string.Equals(login.Identifier.Id, login.Password, StringComparison.Ordinal)
				&& login.Password.Length < 1_024
				&& login.Password.ContainsOnlyExtendedAscii();
		}

		static bool ValidateX509Login(X509Login login)
		{
			throw new NotImplementedException();
		}
	}

	private bool ValidateUnixField(ReadOnlySpan<char> text)
	{
		return text.Length <= 255;
	}

	private bool ValidateValidatingUser(
		ILogin? login,
		LoginIdentifier? identifier,
		Dictionary<LoginIdentifier, ILogin> logins)
	{
		return identifier.HasValue
			? login is null && logins.ContainsKey(identifier.Value)
			: login is not null && ValidateLogin(login);
	}
}
