namespace Lapidary.GemBuilder.Definitions;

public unsafe ref struct GciX509LoginArg
{
	/* const */ public byte* netldiHostOrIp;
	/* const */ public byte* netldiNameOrPort;
	/* const */ public byte* privateKey;
	/* const */ public byte* cert;
	/* const */ public byte* caCert;
	/* const */ public byte* extraGemArgs;
	/* const */ public byte* dirArg; // #dir
	/* const */ public byte* logArg; // #log
	public LoginFlags loginFlags;
	public BoolType argsArePemStrings; // TRUE: args are PEM. FALSE: args are file names
	public BoolType executedSessionInit; // output

	/* Expected constructor matches default initialisation (for now).
	public GciX509LoginArg()
	{
		netldiHostOrIp = default;
		netldiNameOrPort = default;
		privateKey = default;
		cert = default;
		caCert = default;
		extraGemArgs = default;
		dirArg = default;
		logArg = default;
		loginFlags = default;
		argsArePemStrings = default;
		executedSessionInit = default;
	}
	*/

	public readonly bool ArgsAreFileNames()
	{
		return argsArePemStrings != 0;
	}
}
