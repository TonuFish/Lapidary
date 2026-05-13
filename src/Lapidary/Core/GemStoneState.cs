using Lapidary.Authentication;
using Lapidary.Converters;
using Lapidary.Converters.Temporary;
using Lapidary.Core.Login;
using System.Collections.Frozen;
using System.Text;

namespace Lapidary.Core;

internal sealed class GemStoneState
{
	// TODO: Thread safety

	internal required bool AddStandardConverters { get; init; }

	[MemberNotNullWhen(true, nameof(ClassConverters), nameof(NumberConverters), nameof(StructConverters))]
	[MemberNotNullWhen(false, nameof(_validatingLogin))]
	internal bool IsInitialised
	{
		get;
		private set
		{
			if (field || !value)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			field = true;
			_validatingLogin = null;
		}
	}

	internal required ReadOnlyMemory<byte> GemService { get; init; }
	internal required ReadOnlyMemory<byte> HostPassword { get; init; }
	internal required ReadOnlyMemory<byte> HostUserId { get; init; }
	internal required Dictionary<LoginIdentifier, LoginData> Logins { get; init; }
	internal required ReadOnlyMemory<byte> StoneName { get; init; }
	internal required List<ILapidaryConverter>? UserDefinedConverters { get; init; }

	[NotNull]
	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? ClassConverters { get; private set; }

	[NotNull]
	internal FrozenDictionary<Oop, ILapidaryConverter>? NumberConverters { get; private set; }

	[NotNull]
	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? StructConverters { get; private set; }

	private readonly Dictionary<GciSession, LoginData> _sessions = [];

	private LoginData? _validatingLogin;

	internal GemStoneState(LoginData validatingLogin)
	{
		_validatingLogin = validatingLogin;
	}

	internal void Initialise()
	{
		if (IsInitialised)
		{
			return;
		}

		var session = Login(_validatingLogin);
		try
		{
			ProcessConverters(session);
			IsInitialised = true;
		}
		catch (Exception ex)
		{
			// TODO: Errors.
		}
		finally
		{
			Logout(session);
		}
	}

	public GemBuilderSession LoginUser(LoginIdentifier identifier)
	{
		if (!Logins.TryGetValue(identifier, out var data))
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}

		return Login(data);
	}

	#region Login code that should be somewhere else

	private GemBuilderSession Login(LoginData data)
	{
		var session = data switch
		{
			BasicLoginData bld => BasicLogin(bld),
			EncryptedLoginData eld => EncryptedLogin(eld),
			X509LoginData xld => X509Login(xld),
			_ => ThrowHelper.GenericExceptionToDetailLater<GemBuilderSession>(),
		};

		_sessions.Add(session, data);
		return new(session, this);

		GciSession BasicLogin(BasicLoginData login)
		{
			return FFI.Login(
				StoneName.Span,
				HostUserId.Span,
				HostPassword.Span,
				GemService.Span,
				login.Username.Span,
				login.Password.Span);
		}

		GciSession EncryptedLogin(EncryptedLoginData login)
		{
			return FFI.LoginEncrypted(
				StoneName.Span,
				HostUserId.Span,
				HostPassword.Span,
				GemService.Span,
				login.Username.Span,
				login.Password.Span);
		}

		GciSession X509Login(X509LoginData login)
		{
			// TODO: This.
			throw new NotImplementedException();
		}
	}

	private void Logout(GciSession session)
	{
		if (_sessions.Remove(session))
		{
			FFI.Logout(session);
		}
	}

	#endregion Login code that should be somewhere else

	#region LIFTED - PENDING REWORKS

	private Oop FindSymbol(GemBuilderSession session, ReadOnlySpan<char> symbol)
	{
		var symbolCount = Encoding.UTF8.GetByteCount(symbol);
		// TODO: Fixed size stackalloc
		Span<byte> symbolBuffer = stackalloc byte[symbolCount + 1];
		Encoding.UTF8.GetBytes(symbol, symbolBuffer);
		symbolBuffer[^1] = 0;
		var oop = FFI.ResolveSymbol(session, symbolBuffer);
		if (oop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericExceptionToDetailLater();
		}
		return oop;
	}

	private void ProcessConverters(GemBuilderSession session)
	{
		if (IsInitialised)
		{
			return;
		}

		Dictionary<ConverterKey, ILapidaryConverter> classConverters = [];
		Dictionary<Oop, ILapidaryConverter> numberConverters = [];
		Dictionary<ConverterKey, ILapidaryConverter> structConverters = [];

		if (UserDefinedConverters is not null)
		{
			ProcessUserConverters(session, classConverters, numberConverters, structConverters);
		}

		FinaliseConverters(classConverters, numberConverters, structConverters);
	}

	private void ProcessUserConverters(
		GemBuilderSession session,
		Dictionary<ConverterKey, ILapidaryConverter>? classConverters = null,
		Dictionary<Oop, ILapidaryConverter>? numberConverters = null,
		Dictionary<ConverterKey, ILapidaryConverter>? structConverters = null)
	{
		foreach (var converter in UserDefinedConverters)
		{
			if (converter.IdentifyingOops.Count == 0 && converter.IdentifyingSymbols.Count == 0)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			HashSet<Oop> targetOops = [.. converter.IdentifyingOops,];

			foreach (var symbol in converter.IdentifyingSymbols)
			{
				_ = targetOops.Add(FindSymbol(session, symbol.AsSpan()));
			}

			if (converter.CanConvertToClass)
			{
				foreach (var oop in targetOops)
				{
					if (!classConverters.TryAdd(new(oop, converter.ConversionType), converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}

			if (converter.CanConvertToNumber)
			{
				foreach (var oop in targetOops)
				{
					if (!numberConverters.TryAdd(oop, converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}

			if (converter.CanConvertToStruct)
			{
				foreach (var oop in targetOops)
				{
					if (!structConverters.TryAdd(new(oop, converter.ConversionType), converter))
					{
						ThrowHelper.GenericExceptionToDetailLater();
					}
				}
			}
		}
	}

	#region Default Converters (TO REFACTOR)

	// TODO: Quick hack job, do it properly.

	private void FinaliseConverters(
		Dictionary<ConverterKey, ILapidaryConverter> classConverters,
		Dictionary<Oop, ILapidaryConverter> numberConverters,
		Dictionary<ConverterKey, ILapidaryConverter> structConverters)
	{
		if (IsInitialised)
		{
			return;
		}

		if (AddStandardConverters)
		{
			AddDefaultClassConverters(classConverters);
			AddDefaultNumberConverters(numberConverters);
			AddDefaultStructConverters(structConverters);
		}

		ClassConverters = classConverters.ToFrozenDictionary();
		NumberConverters = numberConverters.ToFrozenDictionary();
		StructConverters = structConverters.ToFrozenDictionary();
	}

	private void AddDefaultClassConverters(Dictionary<ConverterKey, ILapidaryConverter> classConverters)
	{
		AddConverter(new StandardStringConverter(), classConverters);
		AddConverter(new OtherStringConverter(), classConverters);
	}

	private void AddDefaultNumberConverters(Dictionary<Oop, ILapidaryConverter> numberConverters)
	{
		AddNumberConverter(new IntegerConverter(), numberConverters);
		AddNumberConverter(new FloatConverter(), numberConverters);

		static void AddNumberConverter(ILapidaryConverter converter, Dictionary<Oop, ILapidaryConverter> converters)
		{
			foreach (var oop in converter.IdentifyingOops)
			{
				_ = converters.TryAdd(oop, converter);
			}
		}
	}

	private void AddDefaultStructConverters(Dictionary<ConverterKey, ILapidaryConverter> structConverters)
	{
		// None.
	}

	private void AddConverter(ILapidaryConverter converter, Dictionary<ConverterKey, ILapidaryConverter> converters)
	{
		foreach (var oop in converter.IdentifyingOops)
		{
			_ = converters.TryAdd(new(oop, converter.ConversionType), converter);
		}
	}

	#endregion Default Converters (TO REFACTOR)

	#endregion LIFTED - PENDING REWORKS
}
