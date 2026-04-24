using Lapidary.Authentication;
using Lapidary.Configuration;
using Lapidary.Core.Login;

namespace Lapidary;

public abstract class GemStone<T> where T : GemStone<T>
{
	// TODO: Currently owned sessions? Or is that the <> job?

	private readonly GemStoneConfiguration<T> _configuration;
	private readonly Dictionary<LoginIdentifier, LoginData> _logins = [];
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
		// TODO: Set state, login
		return null!;
	}
}
