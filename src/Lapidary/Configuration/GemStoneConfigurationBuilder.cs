using Lapidary.Authentication;
using Lapidary.Configuration.Validation;
using Lapidary.Converters;

namespace Lapidary.Configuration;

public abstract class GemStoneConfigurationBuilder
{
	internal bool AddStandardConverters { get; private protected set; } = true;
	internal List<ILapidaryConverter> Converters { get; } = [];
	internal string? GemService { get; private protected set; }
	internal string? HostPassword { get; private protected set; }
	internal string? HostUserId { get; private protected set; }
	internal Dictionary<LoginIdentifier, ILogin> IdentifiersToLogins { get; } = [];
	internal string? StoneName { get; private protected set; }
	internal LoginIdentifier? ValidatingIdentifier { get; private protected set; }
	internal ILogin? ValidatingLogin { get; private protected set; }

	private protected readonly GemStoneConfigurationBuilderValidator _validator;

	private protected GemStoneConfigurationBuilder(GemStoneConfigurationBuilderValidator validator)
	{
		_validator = validator;
	}
}
