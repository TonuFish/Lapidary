namespace Lapidary.GemBuilder.Tests;

public sealed class LoginTests : IDisposable
{
	private bool _dispose;
	private GciSession? _session;

	[Fact]
	public void BasicLogin_WithValidCredentials_Works()
	{
		var exception = Record.Exception(() =>
		{
			_session = FFI.Login(
				Constants.Connection.StoneName,
				Constants.Connection.HostUsername,
				Constants.Connection.HostPassword,
				Constants.Connection.GemService,
				Constants.Connection.Username,
				Constants.Connection.Password);
		});
		Assert.Null(exception);
	}

	[Fact]
	public void EncryptedLogin_WithValidCredentials_Works()
	{
		var encryptedPassword = FFI.Encrypt(Constants.Connection.Password);
		var ep = Assert.NotNull(encryptedPassword);

		var exception = Record.Exception(() =>
		{
			_session = FFI.LoginEncrypted(
				Constants.Connection.StoneName,
				Constants.Connection.HostUsername,
				Constants.Connection.HostPassword,
				Constants.Connection.GemService,
				Constants.Connection.Username,
				ep.Span);
		});
		Assert.Null(exception);
	}

	#region Dispose

	private void Dispose(bool disposing)
	{
		if (!_dispose)
		{
			if (disposing && _session.HasValue)
			{
				FFI.Logout(_session.Value);
			}

			_dispose = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	#endregion Dispose
}
