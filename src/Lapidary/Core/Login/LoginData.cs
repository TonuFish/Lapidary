namespace Lapidary.Core.Login;

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
