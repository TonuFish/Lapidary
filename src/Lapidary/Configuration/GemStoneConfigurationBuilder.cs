using Lapidary.Authentication;
using Lapidary.Configuration.Validation;
using Lapidary.Converters;

namespace Lapidary.Configuration;

public abstract class GemStoneConfigurationBuilder
{
	private protected readonly List<ILapidaryConverter> _converters = [];
	private protected readonly GemStoneConfigurationBuilderValidator _validator;

	private protected string? _gemService;
	private protected string? _hostPassword;
	private protected string? _hostUserId;
	private protected bool _skipStandardConverters;
	private protected string? _stoneName;
	private protected LoginIdentifier? _validatingIdentifier;
	private protected ILogin? _validatingLogin;

	private protected GemStoneConfigurationBuilder(GemStoneConfigurationBuilderValidator validator)
	{
		_validator = validator;
	}
}
