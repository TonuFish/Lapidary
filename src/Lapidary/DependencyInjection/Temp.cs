using Lapidary.Converters;
using Lapidary.Converters.Temporary;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Text;

namespace Lapidary.DependencyInjection;

// TODO: Rename FooBase when specialising multi/single user connections

public static class ServiceProviderExtensions
{
	extension(IServiceProvider sp)
	{
		public void InitialiseGemStone<T>()
			where T : GemStone<T>
		{
			var configuration = sp.GetRequiredService<GemStoneConfiguration<T>>();
			configuration.EnsureInitialised();
		}
	}
}

public abstract class GemStone<T> where T : GemStone<T>
{
	// TODO: Currently owned sessions? Or is that the <> job?

	private readonly GemStoneConfiguration<T> _configuration;
	private readonly Dictionary<LoginIdentifier, LoginData> _logins = [];
	private readonly GemStoneState _state;

	protected GemStone(GemStoneConfiguration<T> configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration);

		var state = configuration.State;
		if (!state.IsInitialised)
		{
			throw new InvalidOperationException("TODO");
		}

		_configuration = configuration;
		_state = state;
	}

	public GemContext<T> GetContext(LoginIdentifier identifier)
	{
		// TODO: Set state, login
		return null!;
	}
}

public abstract class GemStoneConfiguration
{
	internal GemStoneState State { get; init; }

	private protected GemStoneConfiguration(GemStoneState state)
	{
		State = state;
	}

	public void EnsureInitialised()
	{
		if (State.IsInitialised)
		{
			return;
		}

		State.Initialise();
	}
}

public sealed class GemStoneConfiguration<T> : GemStoneConfiguration
	where T : GemStone<T>
{
	internal GemStoneConfiguration(GemStoneState state) : base(state)
	{
	}
}

internal sealed class GemStoneState
{
	// TODO: Thread safety

	[MemberNotNullWhen(true, nameof(ClassConverters), nameof(NumberConverters), nameof(StructConverters))]
	[MemberNotNullWhen(false, nameof(_validatingLogin))]
	internal bool IsInitialised
	{
		get;
		private set
		{
			if (field || !value)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			field = true;
		}
	}

	internal required ReadOnlyMemory<byte> GemService { get; init; }
	internal required ReadOnlyMemory<byte> HostPassword { get; init; }
	internal required ReadOnlyMemory<byte> HostUserId { get; init; }
	internal required Dictionary<LoginIdentifier, LoginData> Logins { get; init; }
	internal required ReadOnlyMemory<byte> StoneName { get; init; }
	internal required List<ILapidaryConverter> UserDefinedConverters { get; init; }

	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? ClassConverters { get; private set; }
	internal FrozenDictionary<Oop, ILapidaryConverter>? NumberConverters { get; private set; }
	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? StructConverters { get; private set; }

	private readonly Dictionary<GciSession, LoginData> _sessions = [];

	private LoginData? _validatingLogin;

	internal GemStoneState(LoginData validatingLogin)
	{
		_validatingLogin = validatingLogin;
	}

	internal void Initialise()
	{
		if (IsInitialised)
		{
			return;
		}

		// TODO: Session here and provide for converter hookup.
		ProcessConverters(null!);
		IsInitialised = true;
	}

	private GemBuilderSession GetValidatingUserSession()
	{
		// TODO: This - clear validating too?
		return Login(_validatingLogin);
	}

	#region Login code that should be somewhere else

	private GemBuilderSession Login(LoginData data)
	{
		// TODO: Switch on login type
		// TODO: Session tracking

		var session = data switch
		{
			BasicLoginData bld => BasicLogin(bld),
			EncryptedLoginData eld => EncryptedLogin(eld),
			X509LoginData xld => X509Login(xld),
			_ => ThrowHelper.GenericExceptionToDetailLater<GemBuilderSession>(),
		};

		_sessions.Add(session, data);
		return new(session, this);

		GciSession BasicLogin(BasicLoginData login)
		{
			return FFI.Login(
				StoneName.Span,
				HostUserId.Span,
				HostPassword.Span,
				GemService.Span,
				login.Username.Span,
				login.Password.Span);
		}

		GciSession EncryptedLogin(EncryptedLoginData login)
		{
			return FFI.LoginEncrypted(
				StoneName.Span,
				HostUserId.Span,
				HostPassword.Span,
				GemService.Span,
				login.Username.Span,
				login.Password.Span);
		}

		GciSession X509Login(X509LoginData login)
		{
			// TODO: This.
			throw new NotImplementedException();
		}
	}

	private void Logout(GciSession session)
	{
		// TODO: Session tracking etc.
		FFI.Logout(session);
	}

	#endregion Login code that should be somewhere else

	#region LIFTED - PENDING REWORKS

	private Oop FindSymbol(GemBuilderSession session, ReadOnlySpan<char> symbol)
	{
		var symbolCount = Encoding.UTF8.GetByteCount(symbol);
		// TODO: Fixed size stackalloc
		Span<byte> symbolBuffer = stackalloc byte[symbolCount + 1];
		Encoding.UTF8.GetBytes(symbol, symbolBuffer);
		symbolBuffer[^1] = 0;
		var oop = FFI.ResolveSymbol(session, symbolBuffer);
		if (oop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}
		return oop;
	}

	private void ProcessConverters(GemBuilderSession session)
	{
		if (IsInitialised)
		{
			return;
		}

		if (UserDefinedConverters is null)
		{
			FinaliseConverters();
			return;
		}

		Dictionary<ConverterKey, ILapidaryConverter> classConverters = [];
		Dictionary<Oop, ILapidaryConverter> numberConverters = [];
		Dictionary<ConverterKey, ILapidaryConverter> structConverters = [];

		foreach (var converter in UserDefinedConverters)
		{
			if (converter.IdentifyingOops.Count == 0 && converter.IdentifyingSymbols.Count == 0)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			HashSet<Oop> targetOops = [.. converter.IdentifyingOops];

			foreach (var symbol in converter.IdentifyingSymbols)
			{
				targetOops.Add(FindSymbol(session, symbol.AsSpan()));
			}

			if (converter.CanConvertToClass)
			{
				foreach (var oop in targetOops)
				{
					if (!classConverters.TryAdd(new(oop, converter.ConversionType), converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}

			if (converter.CanConvertToNumber)
			{
				foreach (var oop in targetOops)
				{
					if (!numberConverters.TryAdd(oop, converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}

			if (converter.CanConvertToStruct)
			{
				foreach (var oop in targetOops)
				{
					if (!structConverters.TryAdd(new(oop, converter.ConversionType), converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}
		}

		FinaliseConverters(classConverters, numberConverters, structConverters);
	}

	#region Default Converters (TO REFACTOR)

	// TODO: Quick hack job, do it properly.

	private void FinaliseConverters(
		Dictionary<ConverterKey, ILapidaryConverter>? classConverters = null,
		Dictionary<Oop, ILapidaryConverter>? numberConverters = null,
		Dictionary<ConverterKey, ILapidaryConverter>? structConverters = null)
	{
		if (IsInitialised)
		{
			return;
		}

		classConverters ??= [];
		numberConverters ??= [];
		structConverters ??= [];

		AddDefaultClassConverters(classConverters);
		AddDefaultNumberConverters(numberConverters);
		AddDefaultStructConverters(structConverters);

		ClassConverters = classConverters.ToFrozenDictionary();
		NumberConverters = numberConverters.ToFrozenDictionary();
		StructConverters = structConverters.ToFrozenDictionary();

		_validatingLogin = null;
		IsInitialised = true;
	}

	private void AddDefaultClassConverters(Dictionary<ConverterKey, ILapidaryConverter> classConverters)
	{
		classConverters.EnsureCapacity(2);

		StandardStringConverter a0 = new();
		foreach (var oop in a0.IdentifyingOops)
		{
			classConverters.TryAdd(new(oop, a0.ConversionType), a0);
		}

		OtherStringConverter a1 = new();
		foreach (var oop in a1.IdentifyingOops)
		{
			classConverters.TryAdd(new(oop, a1.ConversionType), a1);
		}
	}

	private void AddDefaultNumberConverters(Dictionary<Oop, ILapidaryConverter> numberConverters)
	{
		numberConverters.EnsureCapacity(2);

		IntegerConverter a0 = new();
		foreach (var oop in a0.IdentifyingOops)
		{
			numberConverters.TryAdd(oop, a0);
		}

		FloatConverter a1 = new();
		foreach (var oop in a1.IdentifyingOops)
		{
			numberConverters.TryAdd(oop, a1);
		}
	}

	private void AddDefaultStructConverters(Dictionary<ConverterKey, ILapidaryConverter> structConverters)
	{
		// None.
	}

	#endregion Default Converters (TO REFACTOR)

	#endregion LIFTED - PENDING REWORKS
}

internal abstract class LoginData
{
	// TODO: Thread safety
	internal DateTime LastLoginUtc { get; private set; }

	private readonly List<GciSession> _sessions = [];

	internal void AddSession(GciSession session)
	{
		_sessions.Add(session);
		LastLoginUtc = DateTime.UtcNow;
	}

	internal void RemoveSession(GciSession session)
	{
		_ = _sessions.Remove(session);
	}
}

internal sealed class BasicLoginData : LoginData
{
	internal required ReadOnlyMemory<byte> Password { get; init; }
	internal required ReadOnlyMemory<byte> Username { get; init; }
}

internal sealed class EncryptedLoginData : LoginData
{
	internal required ReadOnlyMemory<byte> Password { get; init; }
	internal required ReadOnlyMemory<byte> Username { get; init; }
}

internal sealed class X509LoginData : LoginData
{
	// TODO
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
	where T : GemStone<T>
{
	private readonly Dictionary<LoginIdentifier, ILogin> _identifiersToLogins = [];

	public GemStoneConfigurationBuilder() : base(new GemStoneConfigurationBuilderValidator())
	{
	}

	public GemStoneConfiguration<T> Build()
	{
		Validate();

		Dictionary<LoginIdentifier, LoginData> logins = new(capacity: _identifiersToLogins.Count);
		foreach (var (id, login) in _identifiersToLogins)
		{
			logins[id] = ToData(login);
		}

		GemStoneState todo = new(ToData(_validatingLogin!))
		{
			GemService = ToNullTerminatedAsciiBytes(_gemService),
			HostPassword = ToNullTerminatedAsciiBytes(_hostPassword),
			HostUserId = ToNullTerminatedAsciiBytes(_hostUserId),
			Logins = logins,
			StoneName = ToNullTerminatedAsciiBytes(_stoneName),
			UserDefinedConverters = _converters,
		};

		return new(todo);
	}

	private LoginData ToData(ILogin login)
	{
		return login switch
		{
			BasicLogin { IsEncrypted: false, } el => new BasicLoginData()
			{
				Password = ToNullTerminatedAsciiBytes(el.Password),
				Username = ToNullTerminatedAsciiBytes(el.Username),
			},
			BasicLogin bl => new EncryptedLoginData()
			{
				// TODO: Actually encrypt, below
				Password = ToNullTerminatedAsciiBytes(bl.Password),
				Username = ToNullTerminatedAsciiBytes(bl.Username),
			},
			X509Login xl => new X509LoginData(), // TODO: This.
			_ => ThrowHelper.GenericExceptionToDetailLater<LoginData>(),
		};
	}

	//public EncryptedLoginCredentials EncryptCredentials(BasicLoginCredentials loginBucket)
	//{
	//	var bufferSize = Encoding.UTF8.GetByteCount(loginBucket.Password);

	//	// TODO: Fixed size stackalloc
	//	Span<byte> buffer = stackalloc byte[bufferSize + 1];
	//	Encoding.UTF8.GetBytes(loginBucket.Password.AsSpan(), buffer);
	//	buffer[^1] = 0;

	//	var encryptedBuffer = FFI.Encrypt(buffer);
	//	if (!encryptedBuffer.HasValue)
	//	{
	//		ThrowHelper.GenericExceptionToDetailLater();
	//	}

	//	Memory<byte> usernameBuffer = new(new byte[Encoding.UTF8.GetByteCount(loginBucket.Username) + 1]);
	//	Encoding.UTF8.GetBytes(loginBucket.Username, usernameBuffer.Span);
	//	usernameBuffer.Span[^1] = 0;

	//	return new(usernameBuffer, encryptedBuffer.Value);
	//}

	private void Validate()
	{
		// TODO: This entirely.
		_validator.Validate(this);
	}

	private ReadOnlyMemory<byte> ToNullTerminatedAsciiBytes(ReadOnlySpan<char> text)
	{
		// TODO: Make sure this doesn't need to be UTF8 for GemStone/login details
		// https://downloads.gemtalksystems.com/docs/GemStone64/3.7.x/GS64-SysAdminGuide-3.7/MAIN.htm
		var length = Encoding.ASCII.GetByteCount(text);
		var buffer = new byte[length + 1];
		_ = Encoding.ASCII.GetBytes(text, buffer);
		return buffer;
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

internal abstract class GemStoneConfigurationBuilderValidatorBase
{
	public abstract void Validate<T>(GemStoneConfigurationBuilder<T> builder) where T : GemStone<T>;

	// TODO: Shared validation methods.
}

internal sealed class GemStoneConfigurationBuilderValidator : GemStoneConfigurationBuilderValidatorBase
{
	public override void Validate<T>(GemStoneConfigurationBuilder<T> builder)
	{
		throw new NotImplementedException();
	}
}

// TODO: Rehome orphaned code
//public static string GetGemBuilderVersion()
//{
//	Span<byte> buffer = stackalloc byte[128];
//	buffer.Clear();
//	return FFI.GetGemBuilderVersion(buffer).DecodeUTF8();
//}
