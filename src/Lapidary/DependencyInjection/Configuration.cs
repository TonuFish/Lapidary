using Microsoft.Extensions.DependencyInjection;

namespace Lapidary.DependencyInjection;

public static class Configuration
{
	public static IServiceCollection AddGemStone<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
		this IServiceCollection services,
		Action<GemStoneConfigurationBuilder<T>> configurationBuilderAction)
		where T : GemStone<T>
	{
		ArgumentNullException.ThrowIfNull(configurationBuilderAction);

		GemStoneConfigurationBuilder<T> configurationBuilder = new();
		configurationBuilderAction(configurationBuilder);
		var configuration = configurationBuilder.Build();

		// TODO: This is ugly, change it at some point.
		_ = services.AddSingleton(_ => configuration);

		// TODO: Stop this needing dynamic constructor access.
		_ = services.AddScoped<T>();

		return services;
	}
}
