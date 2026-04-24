using Lapidary.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lapidary.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	extension(IServiceCollection services)
	{
		public IServiceCollection AddGemStone<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
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
}
