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
		}
	}

	internal required ReadOnlyMemory<byte> GemService { get; init; }
	internal required ReadOnlyMemory<byte> HostPassword { get; init; }
	internal required ReadOnlyMemory<byte> HostUserId { get; init; }
	internal required Dictionary<LoginIdentifier, LoginData> Logins { get; init; }
	internal required ReadOnlyMemory<byte> StoneName { get; init; }
	internal required List<ILapidaryConverter> UserDefinedConverters { get; init; }

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

		// TODO: Session here and provide for converter hookup.
		ProcessConverters(null!);
		IsInitialised = true;
	}

	private GemBuilderSession GetValidatingUserSession()
	{
		// TODO: This - clear validating too?
		return Login(_validatingLogin);
	}

	#region Login code that should be somewhere else

	private GemBuilderSession Login(LoginData data)
	{
		// TODO: Switch on login type
		// TODO: Session tracking

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
		// TODO: Session tracking etc.
		FFI.Logout(session);
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

		if (UserDefinedConverters is null)
		{
			FinaliseConverters();
			return;
		}

		Dictionary<ConverterKey, ILapidaryConverter> classConverters = [];
		Dictionary<Oop, ILapidaryConverter> numberConverters = [];
		Dictionary<ConverterKey, ILapidaryConverter> structConverters = [];

		foreach (var converter in UserDefinedConverters)
		{
			if (converter.IdentifyingOops.Count == 0 && converter.IdentifyingSymbols.Count == 0)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			HashSet<Oop> targetOops = [.. converter.IdentifyingOops];

			foreach (var symbol in converter.IdentifyingSymbols)
			{
				targetOops.Add(FindSymbol(session, symbol.AsSpan()));
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

		FinaliseConverters(classConverters, numberConverters, structConverters);
	}

	#region Default Converters (TO REFACTOR)

	// TODO: Quick hack job, do it properly.

	private void FinaliseConverters(
		Dictionary<ConverterKey, ILapidaryConverter>? classConverters = null,
		Dictionary<Oop, ILapidaryConverter>? numberConverters = null,
		Dictionary<ConverterKey, ILapidaryConverter>? structConverters = null)
	{
		if (IsInitialised)
		{
			return;
		}

		classConverters ??= [];
		numberConverters ??= [];
		structConverters ??= [];

		AddDefaultClassConverters(classConverters);
		AddDefaultNumberConverters(numberConverters);
		AddDefaultStructConverters(structConverters);

		ClassConverters = classConverters.ToFrozenDictionary();
		NumberConverters = numberConverters.ToFrozenDictionary();
		StructConverters = structConverters.ToFrozenDictionary();

		_validatingLogin = null;
		IsInitialised = true;
	}

	private void AddDefaultClassConverters(Dictionary<ConverterKey, ILapidaryConverter> classConverters)
	{
		classConverters.EnsureCapacity(2);

		StandardStringConverter a0 = new();
		foreach (var oop in a0.IdentifyingOops)
		{
			classConverters.TryAdd(new(oop, a0.ConversionType), a0);
		}

		OtherStringConverter a1 = new();
		foreach (var oop in a1.IdentifyingOops)
		{
			classConverters.TryAdd(new(oop, a1.ConversionType), a1);
		}
	}

	private void AddDefaultNumberConverters(Dictionary<Oop, ILapidaryConverter> numberConverters)
	{
		numberConverters.EnsureCapacity(2);

		IntegerConverter a0 = new();
		foreach (var oop in a0.IdentifyingOops)
		{
			numberConverters.TryAdd(oop, a0);
		}

		FloatConverter a1 = new();
		foreach (var oop in a1.IdentifyingOops)
		{
			numberConverters.TryAdd(oop, a1);
		}
	}

	private void AddDefaultStructConverters(Dictionary<ConverterKey, ILapidaryConverter> structConverters)
	{
		// None.
	}

	#endregion Default Converters (TO REFACTOR)

	#endregion LIFTED - PENDING REWORKS
}
