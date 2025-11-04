using Lapidary.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapidary.DependencyInjection;

public abstract class GemStone
{
	private readonly ReadOnlyMemory<byte> _gemService;
	private readonly ReadOnlyMemory<byte> _hostPassword;
	private readonly ReadOnlyMemory<byte> _hostUserId;
	private readonly ReadOnlyMemory<byte> _stoneName;

	private readonly Dictionary<string, LoginData> _logins;

	protected GemStone(GemStoneConfiguration<GemStone> gemStoneConfiguration)
	{
		_gemService = gemStoneConfiguration.GemService;
		_hostPassword = gemStoneConfiguration.HostPassword;
		_hostUserId = gemStoneConfiguration.HostUserId;
		_logins = gemStoneConfiguration.Logins;
		_stoneName = gemStoneConfiguration.StoneName;

	}
}

public sealed class GemStoneConfiguration<T>
	where T : GemStone
{
	//! Finalised copy of settings. --- Converters are a bit of a `?`

	internal ReadOnlyMemory<byte> GemService { get; init; }
	internal ReadOnlyMemory<byte> HostPassword { get; init; }
	internal ReadOnlyMemory<byte> HostUserId { get; init; }
	internal Dictionary<string, LoginData> Logins { get; init; }
	internal ReadOnlyMemory<byte> StoneName { get; init; }

	internal GemStoneConfiguration()
	{
	}
}

internal sealed class LoginData
{
	public bool IsEncrypted { get; init; }
	internal ReadOnlyMemory<byte> Password { get; init; }
	internal ReadOnlyMemory<byte> Username { get; init; }
}

public sealed class GemStoneConfigurationBuilder<T>
	where T : GemStone
{
	private readonly List<ILapidaryConverter> _converters = [];
	private readonly Dictionary<LoginIdentifier, ILogin> _identifiersToLogins = [];

	private string? _gemService;
	private string? _hostPassword;
	private string? _hostUserId;
	private bool _skipStandardConverters;
	private string? _stoneName;
	private LoginIdentifier? _validatingIdentifier;
	private ILogin? _validatingLogin;

	public GemStoneConfiguration<T> Build()
	{
		// TODO: This.
		return default;
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

	public GemStoneConfigurationBuilder<T> WithUserLogins(IEnumerable<KeyValuePair<LoginIdentifier, ILogin>> logins)
	{
		foreach ((var identifier, var login) in logins)
		{
			_ = _identifiersToLogins.TryAdd(identifier, login);
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
	internal static LapidaryProvider Instance { get; } = new();

	// TODO: DB bucket -> Configs, Sessions, Logins

	private LapidaryProvider()
	{
	}
}
