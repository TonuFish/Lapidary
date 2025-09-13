using Lapidary.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lapidary.Samples;

internal static class BasicExample
{
	public static void Foo()
	{
		var hostBuilder = Host.CreateApplicationBuilder();

		// Add services to DI.
		hostBuilder.Services.AddLapidaryServices();

		var host = hostBuilder.Build();

		// Configure your database.
		var manager = host.Services.GetRequiredService<ILapidaryManagementService>();

		const string serverAddress = "localhost";
		const string netLdiPort = "50377";
		const string databaseName = "gs64stone";

		DatabaseConnectionCredentials dbcc = new(
			gemService: $"!tcp@{serverAddress}#netldi:{netLdiPort}#task!gemnetobject",
			stoneName: $"!@{serverAddress}!{databaseName}")
		{
			HostPassword = "",
			HostUserId = "",
		};
		DatabaseIdentifier dbid = new(name: "Fish");

		manager.ConfigureDatabase(dbid, dbcc);

		// Configure your persistent session.
		var encryptedCredentials = manager.EncryptCredentials(new("DataCurator", "swordfish"));
		var userSession = manager.LoginEncrypted(dbid, encryptedCredentials);

		// Retrieve your session.
		var factory = host.Services.GetRequiredService<IGemContextFactory>();
		var context = factory.GetContext(userSession);

		// Use your connection.
		var aObject = context.PerformSmalltalkRaw("72"u8);
		var bObject = context.PerformSmalltalkRaw("43"u8);
		var result = aObject.Perform("+"u8, bObject);

		// Read some objects.
		var resultAsText = result.Perform("printString"u8).GetString();
		var resultAsNumber = result.GetNumber<int>();

		// Disconnect your session.
		manager.Logout(userSession);
	}
}
