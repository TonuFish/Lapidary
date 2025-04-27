using Microsoft.Extensions.DependencyInjection;

namespace Lapidary.GemBuilder.DependencyInjection;

public static class Configuration
{
    public static IServiceCollection AddLapidaryServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ILapidaryManagementService, LapidaryManagementService>()
            .AddSingleton<IGemContextFactory, GemContextFactory>();
    }
}
