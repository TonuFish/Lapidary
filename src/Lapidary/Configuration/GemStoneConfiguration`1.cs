namespace Lapidary.Configuration;

public sealed class GemStoneConfiguration<T> : GemStoneConfiguration
	where T : GemStone<T>
{
	internal GemStoneConfiguration(GemStoneState state) : base(state)
	{
	}
}
