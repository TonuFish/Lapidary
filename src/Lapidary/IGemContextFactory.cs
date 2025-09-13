namespace Lapidary;

public interface IGemContextFactory
{
    public GemContext GetContext(SessionIdentifier sessionIdentifier);
}
