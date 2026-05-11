using Lapidary.Authentication;
using Lapidary.Converters;
using Lapidary.Core.Login;

namespace Lapidary.Configuration;

public sealed class GemStoneConfigurationBuilder<T> : GemStoneConfigurationBuilder
	where T : GemStone<T>
{
	public GemStoneConfigurationBuilder() : base(new())
	{
	}

	public GemStoneConfiguration<T> Build()
	{
		PrepareAndValidate();

		Dictionary<LoginIdentifier, LoginData> logins = new(capacity: IdentifiersToLogins.Count);
		foreach (var (id, login) in IdentifiersToLogins)
		{
			logins[id] = LoginData.Create(login);
		}

		// TODO: If using a validating identifier the credentials get created twice.
		GemStoneState state = new(LoginData.Create(ValidatingLogin!))
		{
			GemService = GemService.ToNullTerminatedBytes(),
			HostPassword = HostPassword.ToNullTerminatedBytes(),
			HostUserId = HostUserId.ToNullTerminatedBytes(),
			Logins = logins,
			StoneName = StoneName.ToNullTerminatedBytes(),
			UserDefinedConverters = Converters,
		};

		return new(state);
	}

	public GemStoneConfigurationBuilder<T> ConfigureConnection(
		string gemService,
		string stoneName,
		string? hostUserId = null,
		string? hostPassword = null)
	{
		GemService = gemService;
		StoneName = stoneName;
		HostPassword = hostPassword;
		HostUserId = hostUserId;
		return this;
	}

	public GemStoneConfigurationBuilder<T> SkipStandardConverters()
	{
		AddStandardConverters = false;
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithConverters(IEnumerable<ILapidaryConverter> converters)
	{
		ArgumentNullException.ThrowIfNull(converters);

		Converters.AddRange(converters);
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithUserLogins(IEnumerable<ILogin> logins)
	{
		ArgumentNullException.ThrowIfNull(logins);

		foreach (var login in logins)
		{
			// TODO: Consider if failing should throw for clarity - first added takes priority.
			_ = IdentifiersToLogins.TryAdd(login.Identifier, login);
		}
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithValidatingLogin(ILogin login)
	{
		ArgumentNullException.ThrowIfNull(login);

		ValidatingLogin = login;
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithValidatingLogin(LoginIdentifier identifier)
	{
		ValidatingIdentifier = identifier;
		return this;
	}

	private void PrepareAndValidate()
	{
		_validator.Validate(this);
		ValidatingLogin ??= IdentifiersToLogins[ValidatingIdentifier!.Value];
	}
}
