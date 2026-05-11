using Lapidary.Authentication;
using Lapidary.Configuration;

namespace Lapidary;

public abstract class GemStone<T> where T : GemStone<T>
{
	// TODO: Currently owned sessions? Or is that the <> job?

	private readonly GemStoneConfiguration<T> _configuration;
	private readonly GemStoneState _state;

	protected GemStone(GemStoneConfiguration<T> configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration);

		var state = configuration.State;
		if (!state.IsInitialised)
		{
			throw new InvalidOperationException("TODO");
		}

		_configuration = configuration;
		_state = state;
	}

	public GemContext<T> GetContext(LoginIdentifier identifier)
	{
		// TODO: Track sessions on stone?
		var session = _state.LoginUser(identifier);
		return new(session);
	}
}
