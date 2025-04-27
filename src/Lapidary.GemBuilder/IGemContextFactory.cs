namespace Lapidary.GemBuilder;

public interface IGemContextFactory
{
    public GemContext GetContext(SessionIdentifier sessionIdentifier);
}
