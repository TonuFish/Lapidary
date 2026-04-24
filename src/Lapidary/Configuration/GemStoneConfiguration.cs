namespace Lapidary.Configuration;

public abstract class GemStoneConfiguration
{
	internal GemStoneState State { get; init; }

	private protected GemStoneConfiguration(GemStoneState state)
	{
		State = state;
	}

	public void EnsureInitialised()
	{
		if (State.IsInitialised)
		{
			return;
		}

		State.Initialise();
	}
}
