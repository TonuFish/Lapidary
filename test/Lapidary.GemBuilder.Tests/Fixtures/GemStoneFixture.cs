namespace Lapidary.GemBuilder.Tests.Fixtures;

public sealed class GemStoneFixture : IDisposable
{
	public GciSession Session { get; }

	private bool _disposed;

	public GemStoneFixture()
	{
		Session = FFI.Login(
			Constants.Connection.StoneName,
			Constants.Connection.HostUsername,
			Constants.Connection.HostPassword,
			Constants.Connection.GemService,
			Constants.Connection.Username,
			Constants.Connection.Password);
	}

	#region Dispose

	private void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				FFI.Logout(Session);
			}

			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	#endregion Dispose
}
