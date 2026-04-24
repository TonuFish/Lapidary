using Lapidary.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lapidary.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
	extension(IServiceProvider sp)
	{
		public void InitialiseGemStone<T>()
			where T : GemStone<T>
		{
			var configuration = sp.GetRequiredService<GemStoneConfiguration<T>>();
			configuration.EnsureInitialised();
		}
	}
}
