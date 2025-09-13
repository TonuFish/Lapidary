using System.Collections.Concurrent;

namespace Lapidary.DependencyInjection;

public sealed class GemContextFactory : IGemContextFactory
{
    private readonly ConcurrentDictionary<SessionIdentifier, GemBuilderSession> _sessions = new();

    public GemContext GetContext(SessionIdentifier sessionIdentifier)
    {
        if (!_sessions.TryGetValue(sessionIdentifier, out var session))
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return new(session);
    }

    internal SessionIdentifier AddContext(GemBuilderSession session)
    {
        SessionIdentifier newIdentifier = new(Guid.NewGuid());
        _sessions[newIdentifier] = session;
        return newIdentifier;
    }

    internal GemBuilderSession? RemoveContext(SessionIdentifier sessionIdentifier)
    {
        return _sessions.Remove(sessionIdentifier, out var session) ? session : null;
    }
}
