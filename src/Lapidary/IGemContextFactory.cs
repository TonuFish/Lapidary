using Lapidary.DependencyInjection;

namespace Lapidary;

public interface IGemContextFactory
{
	public GemContext<T> GetContext<T>(SessionIdentifier sessionIdentifier) where T : GemStone<T>;
}
