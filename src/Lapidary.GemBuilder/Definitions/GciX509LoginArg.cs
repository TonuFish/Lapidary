namespace Lapidary.GemBuilder.Definitions;

internal unsafe ref struct GciX509LoginArg
{
	/* const */ internal byte* netldiHostOrIp;
	/* const */ internal byte* netldiNameOrPort;
	/* const */ internal byte* privateKey;
	/* const */ internal byte* cert;
	/* const */ internal byte* caCert;
	/* const */ internal byte* extraGemArgs;
	/* const */ internal byte* dirArg; // #dir
	/* const */ internal byte* logArg; // #log
	internal LoginFlags loginFlags;
	internal BoolType argsArePemStrings; // TRUE: args are PEM. FALSE: args are file names
	internal BoolType executedSessionInit; // output

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
}
