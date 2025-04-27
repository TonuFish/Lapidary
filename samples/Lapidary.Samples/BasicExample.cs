using Lapidary.GemBuilder;
using Lapidary.GemBuilder.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lapidary.Samples;

public static class BasicExample
{
	public static void Foo()
	{
		var hostBuilder = Host.CreateApplicationBuilder();

		// Add services to DI.
		hostBuilder.Services.AddLapidaryServices();

		var host = hostBuilder.Build();

		// Configure your database.
		var manager = host.Services.GetRequiredService<ILapidaryManagementService>();

		DatabaseConnectionCredentials dbcc = new(gemService: "", stoneName: "")
		{
			HostPassword = "",
			HostUserId = "",
		};
		DatabaseIdentifier dbid = new(name: "Fish");

		manager.ConfigureDatabase(dbid, dbcc);

		// Configure your persistent session.
		var encryptedCredentials = manager.EncryptCredentials(new("SystemUser", "swordfish"));
		var userSession = manager.LoginEncrypted(dbid, encryptedCredentials);

		// Retrieve your session.
		var factory = host.Services.GetRequiredService<IGemContextFactory>();
		var context = factory.GetContext(userSession);

		// User your connection.
		var aObject = context.PerformSmalltalkRaw("a"u8);
		var bObject = context.PerformSmalltalkRaw("b"u8);
		var result = aObject.Perform("something"u8, bObject);

		// Read some objects.
		var resultAsText = result.GetString();
		var resultAsNumber = result.GetNumber<int>();
		var resultAsDateTime = result.GetDateTime();

		// Disconnect your session.
		manager.Logout(userSession);
	}
}
