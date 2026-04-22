using Lapidary.Converters;
using Lapidary.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Lapidary.Samples;

internal static class ReworkExample
{
	public static void Bar()
	{
		var configuration = new GemStoneConfigurationBuilder<FooGemStone>()
			.ConfigureConnection(
				gemService: "!tcp@localhost#netldi:50377#task!gemnetobject",
				stoneName: "!@localhost!gs64stone")
			.SkipStandardConverters()
			.WithConverters(
				[
					new FooHalfConverter(),
				])
			.WithUserLogins(
				[
					new BasicLogin(new("foo"), "DataCurator", "swordfish"),
				])
			.WithValidatingLogin(identifier: new("foo"))
			.Build();

		configuration.EnsureInitialised();

		FooGemStone gemStone = new(configuration);
		Use(gemStone);
	}

	public static void Foo()
	{
		var hostBuilder = Host.CreateApplicationBuilder();

		_ = hostBuilder.Services.AddGemStone<FooGemStone>(configuration =>
			configuration
				.ConfigureConnection(
					gemService: "!tcp@localhost#netldi:50377#task!gemnetobject",
					stoneName: "!@localhost!gs64stone")
				.SkipStandardConverters()
				.WithConverters(
					[
						new FooHalfConverter(),
					])
				.WithUserLogins(
					[
						new BasicLogin(new("foo"), "DataCurator", "swordfish"),
					])
				.WithValidatingLogin(identifier: new("foo")));

		var host = hostBuilder.Build();

		host.Services.InitialiseGemStone<FooGemStone>();

		var gemStone = host.Services.GetRequiredService<FooGemStone>();
		Use(gemStone);
	}

	public static void Use(FooGemStone gemStone)
	{
		var context = gemStone.GetContext(new("foo"));

		// Use your connection.
		var aObject = context.PerformSmalltalkRaw("72"u8);
		var bObject = context.PerformSmalltalkRaw("43"u8);
		var result = aObject.Perform("+"u8, bObject);

		// Read some objects.
		var resultAsText = result.Perform("printString"u8).GetString();
		var resultAsNumber = result.GetNumber<int>();
	}
}

public sealed class FooGemStone : GemStone<FooGemStone>
{
	public FooGemStone(GemStoneConfiguration<FooGemStone> configuration) : base(configuration)
	{
	}
}

public class FooHalfConverter : LapidaryNumberConverter<Half>
{
	public override IList<ulong> IdentifyingOops => [];

	public override IList<string> IdentifyingSymbols => ["Half",];

	protected override ConversionResult<Half> ConvertObject(GemObject gemObject)
	{
		return ConversionResult.FromResult(Half.Zero);
	}
}
