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
			.WithConverters(
			[
				new FooHalfConverter(),
			])
			.SkipStandardConverters()
			.WithUserLogins(
			[
				new BasicLogin(new("foo"), "DataCurator", "swordfish"),
			])
			.WithValidatingLogin(identifier: new("foo"))
			.Build();

		FooGemStone myGsDatabase = new(configuration);
	}

	public static void Foo()
	{
		var hostBuilder = Host.CreateApplicationBuilder();

		_ = hostBuilder.Services.AddGemStone<FooGemStone>(configuration =>
			configuration
				.ConfigureConnection(
					gemService: "!tcp@localhost#netldi:50377#task!gemnetobject",
					stoneName: "!@localhost!gs64stone")
				.WithConverters(
					[
						new FooHalfConverter(),
					])
				.SkipStandardConverters()
				.WithUserLogins(
					[
						new BasicLogin(new("foo"), "DataCurator", "swordfish"),
					])
				.WithValidatingLogin(identifier: new("foo")));

		var host = hostBuilder.Build();

		var myGsDatabase = host.Services.GetRequiredService<FooGemStone>();
	}
}

public sealed class FooGemStone : GemStone
{
	public FooGemStone(GemStoneConfiguration<FooGemStone> gemStoneConfiguration) : base(gemStoneConfiguration)
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
