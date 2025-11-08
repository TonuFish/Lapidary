using Lapidary.Converters;

namespace Lapidary.DependencyInjection;

// TODO: Rename FooBase when specialising multi/single user connections

public abstract class GemStone
{
	private readonly ReadOnlyMemory<byte> _gemService;
	private readonly ReadOnlyMemory<byte> _hostPassword;
	private readonly ReadOnlyMemory<byte> _hostUserId;
	private readonly LapidaryProvider _provider;
	private readonly ReadOnlyMemory<byte> _stoneName;

	private readonly Dictionary<LoginIdentifier, LoginData> _logins;

	protected GemStone(GemStoneConfiguration gemStoneConfiguration)
	{
		ArgumentNullException.ThrowIfNull(gemStoneConfiguration);

		_gemService = gemStoneConfiguration.GemService;
		_hostPassword = gemStoneConfiguration.HostPassword;
		_hostUserId = gemStoneConfiguration.HostUserId;
		_logins = gemStoneConfiguration.Logins;
		_provider = gemStoneConfiguration.Provider;
		_stoneName = gemStoneConfiguration.StoneName;
	}
}

public abstract class GemStoneConfiguration
{
	//! Finalised copy of settings. --- Converters are a bit of a `?`

	internal ReadOnlyMemory<byte> GemService { get; init; }
	internal ReadOnlyMemory<byte> HostPassword { get; init; }
	internal ReadOnlyMemory<byte> HostUserId { get; init; }
	internal Dictionary<LoginIdentifier, LoginData> Logins { get; init; }
	internal LapidaryProvider Provider { get; init; }
	internal ReadOnlyMemory<byte> StoneName { get; init; }

	private protected GemStoneConfiguration(LapidaryProvider provider)
	{
		Provider = provider;
	}
}

public sealed class GemStoneConfiguration<T> : GemStoneConfiguration
	where T : GemStone
{
	internal GemStoneConfiguration(LapidaryProvider provider) : base(provider)
	{
	}
}

internal sealed class LoginData
{
	public bool IsEncrypted { get; init; }
	internal ReadOnlyMemory<byte> Password { get; init; }
	internal ReadOnlyMemory<byte> Username { get; init; }
}

public abstract class GemStoneConfigurationBuilderBase
{
	private protected readonly List<ILapidaryConverter> _converters = [];
	private protected readonly GemStoneConfigurationBuilderValidatorBase _validator;

	private protected string? _gemService;
	private protected string? _hostPassword;
	private protected string? _hostUserId;
	private protected bool _skipStandardConverters;
	private protected string? _stoneName;
	private protected LoginIdentifier? _validatingIdentifier;
	private protected ILogin? _validatingLogin;

	private protected GemStoneConfigurationBuilderBase(GemStoneConfigurationBuilderValidatorBase validator)
	{
		_validator = validator;
	}
}

public sealed class GemStoneConfigurationBuilder<T> : GemStoneConfigurationBuilderBase
	where T : GemStone
{
	private readonly Dictionary<LoginIdentifier, ILogin> _identifiersToLogins = [];

	public GemStoneConfigurationBuilder() : base(new GemStoneConfigurationBuilderValidator())
	{
	}

	public GemStoneConfiguration<T> Build()
	{
		// TODO: This.
		_validator.Validate(this);
		return new(LapidaryProvider.Instance);
	}

	public GemStoneConfigurationBuilder<T> ConfigureConnection(
		string gemService,
		string stoneName,
		string? hostUserId = null,
		string? hostPassword = null)
	{
		_gemService = gemService;
		_stoneName = stoneName;
		_hostPassword = hostPassword;
		_hostUserId = hostUserId;
		return this;
	}

	public GemStoneConfigurationBuilder<T> SkipStandardConverters()
	{
		_skipStandardConverters = true;
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithConverters(IEnumerable<ILapidaryConverter> converters)
	{
		_converters.AddRange(converters);
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithUserLogins(IEnumerable<ILogin> logins)
	{
		ArgumentNullException.ThrowIfNull(logins);

		foreach (var login in logins)
		{
			// TODO: Consider if failing should throw for clarity - first added takes priority.
			_ = _identifiersToLogins.TryAdd(login.Identifier, login);
		}
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithValidatingLogin(ILogin login)
	{
		_validatingLogin = login;
		return this;
	}

	public GemStoneConfigurationBuilder<T> WithValidatingLogin(LoginIdentifier identifier)
	{
		_validatingIdentifier = identifier;
		return this;
	}
}

public interface ILogin
{
	public LoginIdentifier Identifier { get; }
}

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

public sealed class X509Login : ILogin
{
	// TODO
	public LoginIdentifier Identifier { get; init; }
}

public readonly struct LoginIdentifier : IEquatable<LoginIdentifier>
{
	public required string Id { get; init; }

	public static bool operator ==(LoginIdentifier left, LoginIdentifier right) => left.Equals(right);
	public static bool operator !=(LoginIdentifier left, LoginIdentifier right) => !(left == right);

	[SetsRequiredMembers]
	public LoginIdentifier(string id)
	{
		Id = id;
	}

	public bool Equals(LoginIdentifier other)
	{
		return Id.Equals(other.Id, StringComparison.Ordinal);
	}

	public override bool Equals(object? obj)
	{
		return obj is LoginIdentifier login && Equals(login);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode(StringComparison.Ordinal);
	}
}

internal sealed class LapidaryProvider
{
	/*
	 * Per database.
	 * - Sessions
	 * - Logins
	 */

	private readonly Dictionary<Type, DatabaseThing> _asdf = [];

	internal static LapidaryProvider Instance { get; } = new();

	// TODO: DB bucket -> Configs, Sessions, Logins

	private LapidaryProvider()
	{
	}
}

internal sealed class DatabaseThing
{
}

public abstract class GemStoneConfigurationBuilderValidatorBase
{
	public abstract void Validate<T>(GemStoneConfigurationBuilder<T> builder) where T : GemStone;

	// TODO: Shared validation methods.
}

internal sealed class GemStoneConfigurationBuilderValidator : GemStoneConfigurationBuilderValidatorBase
{
	public override void Validate<T>(GemStoneConfigurationBuilder<T> builder)
	{
		throw new NotImplementedException();
	}
}
