using Lapidary.Authentication;

namespace Lapidary.Core.Login;

internal abstract class LoginData
{
	// TODO: Thread safety
	internal DateTime LastLoginUtc { get; private set; }

	private readonly List<GciSession> _sessions = [];

	internal static LoginData Create(ILogin login)
	{
		return login switch
		{
			BasicLogin { IsEncrypted: true, } e => new EncryptedLoginData()
			{
				Password = e.Password.ToEncryptedNullTerminatedBytes(),
				Username = e.Username.ToNullTerminatedBytes(),
			},
			BasicLogin b => new BasicLoginData()
			{
				Password = b.Password.ToNullTerminatedBytes(),
				Username = b.Username.ToNullTerminatedBytes(),
			},
			X509Login x => new X509LoginData(),
			_ => ThrowHelper.GenericExceptionToDetailLater<LoginData>(),
		};
	}

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
