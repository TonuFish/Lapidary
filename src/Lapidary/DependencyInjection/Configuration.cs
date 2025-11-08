using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lapidary.DependencyInjection;

public static class Configuration
{
	public static IServiceCollection AddLapidaryServices(this IServiceCollection services)
	{
		return services
			.AddSingleton<ILapidaryManagementService, LapidaryManagementService>()
			.AddSingleton<IGemContextFactory, GemContextFactory>();
	}

	public static IServiceCollection AddGemStone<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
		this IServiceCollection services,
		Action<GemStoneConfigurationBuilder<T>> configurationBuilderAction)
		where T : GemStone
	{
		ArgumentNullException.ThrowIfNull(configurationBuilderAction);

		services.TryAddSingleton(LapidaryProvider.Instance);

		GemStoneConfigurationBuilder<T> configurationBuilder = new();
		configurationBuilderAction(configurationBuilder);

		// TODO: Validate builder.

		var configuration = configurationBuilder.Build();

		// TODO: Add things to the provider.

		_ = services.AddScoped(_ => configuration);
		_ = services.AddScoped<T>();

		return services;
	}
}
