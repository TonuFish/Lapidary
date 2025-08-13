using Lapidary.GemBuilder.Converters;
using System.Text;

namespace Lapidary.GemBuilder.DependencyInjection;

internal sealed class LapidaryManagementService : ILapidaryManagementService
{
	private readonly Dictionary<DatabaseIdentifier, DatabaseBucket> _databaseConfigurations = [];
	private readonly GemContextFactory _gemFactory;

	public LapidaryManagementService(IGemContextFactory factory)
	{
		// TODO: Fix this cast.
		_gemFactory = (GemContextFactory)factory;
	}

	public void ConfigureDatabase(
		DatabaseIdentifier identifier,
		DatabaseConnectionCredentials connection,
		IList<ILapidaryConverter>? converters = null)
	{
		// TODO: Converter validation - dupes etc.

		if (_databaseConfigurations.ContainsKey(identifier))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		_databaseConfigurations.Add(
			identifier,
			new()
			{
				Identifier = identifier,
				ConnectionDetails = connection,
				UserDefinedConverters = converters,
			});
	}

	// TODO: Login overhaul - credentials and login methods.

	public EncryptedLoginCredentials EncryptCredentials(LoginCredentials loginBucket)
	{
		var bufferSize = Encoding.UTF8.GetByteCount(loginBucket.Password);

		// TODO: Fixed size stackalloc
		Span<byte> buffer = stackalloc byte[bufferSize + 1];
		Encoding.UTF8.GetBytes(loginBucket.Password.AsSpan(), buffer);
		buffer[^1] = 0;

		var encryptedBuffer = FFI.Encrypt(buffer);
		if (!encryptedBuffer.HasValue)
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		Memory<byte> usernameBuffer = new(new byte[Encoding.UTF8.GetByteCount(loginBucket.Username) + 1]);
		Encoding.UTF8.GetBytes(loginBucket.Username, usernameBuffer.Span);
		usernameBuffer.Span[^1] = 0;

		return new(usernameBuffer, encryptedBuffer.Value);
	}

	public SessionIdentifier Login(DatabaseIdentifier identifier, LoginCredentials credentials)
	{
		if (!_databaseConfigurations.TryGetValue(identifier, out var database))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		// TODO: Write "the ugly" better.

		var username = credentials.Username.AsSpan();
		var usernameCount = Encoding.UTF8.GetByteCount(username);
		Span<byte> usernameBuffer = stackalloc byte[usernameCount + 1];
		Encoding.UTF8.GetBytes(username, usernameBuffer);
		usernameBuffer[^1] = 0;

		var password = credentials.Password.AsSpan();
		var passwordCount = Encoding.UTF8.GetByteCount(password);
		Span<byte> passwordBuffer = stackalloc byte[passwordCount + 1];
		Encoding.UTF8.GetBytes(password, passwordBuffer);
		passwordBuffer[^1] = 0;

		var hostUsername = database.ConnectionDetails.HostUserId.AsSpan();
		var hostUsernameCount = Encoding.UTF8.GetByteCount(hostUsername);
		Span<byte> hostUsernameBuffer = stackalloc byte[hostUsernameCount + 1];
		Encoding.UTF8.GetBytes(hostUsername, hostUsernameBuffer);
		hostUsernameBuffer[^1] = 0;

		var hostPassword = database.ConnectionDetails.HostPassword.AsSpan();
		var hostPasswordCount = Encoding.UTF8.GetByteCount(hostPassword);
		Span<byte> hostPasswordBuffer = stackalloc byte[hostPasswordCount + 1];
		Encoding.UTF8.GetBytes(hostPassword, hostPasswordBuffer);
		hostPasswordBuffer[^1] = 0;

		var stoneName = database.ConnectionDetails.StoneName.AsSpan();
		var stoneNameCount = Encoding.UTF8.GetByteCount(stoneName);
		Span<byte> stoneNameBuffer = stackalloc byte[stoneNameCount + 1];
		Encoding.UTF8.GetBytes(stoneName, stoneNameBuffer);
		stoneNameBuffer[^1] = 0;

		var gemService = database.ConnectionDetails.GemService.AsSpan();
		var gemServiceCount = Encoding.UTF8.GetByteCount(gemService);
		Span<byte> gemServiceBuffer = stackalloc byte[gemServiceCount + 1];
		Encoding.UTF8.GetBytes(gemService, gemServiceBuffer);
		gemServiceBuffer[^1] = 0;

		if (!FFI.Login(
			stoneName: stoneNameBuffer,
			hostUsername: hostUsernameBuffer,
			hostPassword: hostPasswordBuffer,
			gemService: gemServiceBuffer,
			username: usernameBuffer,
			password: passwordBuffer,
			session: out var sessionId,
			error: out var error))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		GemBuilderSession session = new(sessionId.Value)
		{
			Bucket = database
		};

		if (!database.IsPrepared)
		{
			// TODO: Shouldn't be here.
			database.PrepareDatabase(session);
		}

		return _gemFactory.AddContext(session);
	}

	public SessionIdentifier LoginEncrypted(DatabaseIdentifier identifier, EncryptedLoginCredentials credentials)
	{
		if (!_databaseConfigurations.TryGetValue(identifier, out var database))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		// TODO: Write "the ugly" better.

		var hostUsername = database.ConnectionDetails.HostUserId.AsSpan();
		var hostUsernameCount = Encoding.UTF8.GetByteCount(hostUsername);
		Span<byte> hostUsernameBuffer = stackalloc byte[hostUsernameCount + 1];
		Encoding.UTF8.GetBytes(hostUsername, hostUsernameBuffer);
		hostUsernameBuffer[^1] = 0;

		var hostPassword = database.ConnectionDetails.HostPassword.AsSpan();
		var hostPasswordCount = Encoding.UTF8.GetByteCount(hostPassword);
		Span<byte> hostPasswordBuffer = stackalloc byte[hostPasswordCount + 1];
		Encoding.UTF8.GetBytes(hostPassword, hostPasswordBuffer);
		hostPasswordBuffer[^1] = 0;

		var stoneName = database.ConnectionDetails.StoneName.AsSpan();
		var stoneNameCount = Encoding.UTF8.GetByteCount(stoneName);
		Span<byte> stoneNameBuffer = stackalloc byte[stoneNameCount + 1];
		Encoding.UTF8.GetBytes(stoneName, stoneNameBuffer);
		stoneNameBuffer[^1] = 0;

		var gemService = database.ConnectionDetails.GemService.AsSpan();
		var gemServiceCount = Encoding.UTF8.GetByteCount(gemService);
		Span<byte> gemServiceBuffer = stackalloc byte[gemServiceCount + 1];
		Encoding.UTF8.GetBytes(gemService, gemServiceBuffer);
		gemServiceBuffer[^1] = 0;

		if (!FFI.LoginEncrypted(
			stoneName: stoneNameBuffer,
			hostUsername: hostUsernameBuffer,
			hostPassword: hostPasswordBuffer,
			gemService: gemServiceBuffer,
			username: credentials.Username.Span,
			password: credentials.Password.Span,
			session: out var sessionId,
			error: out var error))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		GemBuilderSession session = new(sessionId.Value)
		{
			Bucket = database
		};

		if (!database.IsPrepared)
		{
			// TODO: Shouldn't be here.
			database.PrepareDatabase(session);
		}

		return _gemFactory.AddContext(session);
	}

	public SessionIdentifier LoginX509(DatabaseIdentifier identifier, X509LoginCredentials credentials)
	{
		if (!_databaseConfigurations.TryGetValue(identifier, out var database))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		// TODO: PemStrings==false, validate read on all cert files.

		var netldiHostOrIpBuffer = JustAllocateTheBuffer(credentials.NetldiHostOrIp);
		var netldiNameOrPortBuffer = JustAllocateTheBuffer(credentials.NetldiNameOrPort);
		var privateKeyBuffer = JustAllocateTheBuffer(credentials.PrivateKey);
		var certBuffer = JustAllocateTheBuffer(credentials.Cert);
		var caCertBuffer = JustAllocateTheBuffer(credentials.CaCert);
		var extraGemArgsBuffer = JustAllocateTheBuffer(credentials.ExtraGemArgs);
		var dirArgBuffer = JustAllocateTheBuffer(credentials.DirArg);
		var logArgBuffer = JustAllocateTheBuffer(credentials.LogArg);

		if (!FFI.LoginX509(
			netldiHostOrIp: netldiHostOrIpBuffer,
			netldiNameOrPort: netldiNameOrPortBuffer,
			privateKey: privateKeyBuffer,
			cert: certBuffer,
			caCert: caCertBuffer,
			extraGemArgs: extraGemArgsBuffer,
			dirArg: dirArgBuffer,
			logArg: logArgBuffer,
			argsArePemStrings: credentials.ArgsArePemStrings,
			session: out var sessionId,
			error: out var error))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		GemBuilderSession session = new(sessionId.Value)
		{
			Bucket = database
		};

		if (!database.IsPrepared)
		{
			// TODO: Shouldn't be here.
			database.PrepareDatabase(session);
		}

		return _gemFactory.AddContext(session);

		static ReadOnlySpan<byte> JustAllocateTheBuffer(string text)
		{
			// TODO: All the buffer handling in here is messy as it is, taking the fast way home.

			if (text.Length == 0)
			{
				return [];
			}

			var requiredLength = Encoding.UTF8.GetByteCount(text) + 1;
			var buffer = new byte[requiredLength];
			Encoding.UTF8.GetBytes(text.AsSpan(), buffer.AsSpan());
			buffer[requiredLength - 1] = 0;
			return buffer.AsSpan(0, requiredLength);
		}
	}

	public void Logout(SessionIdentifier sessionIdentifier)
	{
		// TODO: Errors.
		var session = _gemFactory.RemoveContext(sessionIdentifier);
		if (session is not null)
		{
			FFI.Logout(session);
		}
	}
}
