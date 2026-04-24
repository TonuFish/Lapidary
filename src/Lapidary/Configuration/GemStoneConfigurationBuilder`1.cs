using Lapidary.Authentication;
using Lapidary.Converters;
using Lapidary.Core.Login;
using System.Text;

namespace Lapidary.Configuration;

public sealed class GemStoneConfigurationBuilder<T> : GemStoneConfigurationBuilder
	where T : GemStone<T>
{
	private readonly Dictionary<LoginIdentifier, ILogin> _identifiersToLogins = [];

	public GemStoneConfigurationBuilder() : base(new())
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
			X509Login xl => ThrowHelper.GenericExceptionToDetailLater<LoginData>(), // new X509LoginData(), // TODO: This.
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
