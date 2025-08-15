using Lapidary.Converters;
using Lapidary.Converters.Temporary;
using System.Collections.Frozen;
using System.Text;

namespace Lapidary;

internal sealed class DatabaseBucket
{
	public required DatabaseIdentifier Identifier { get; init; }
	public required DatabaseConnectionCredentials ConnectionDetails { get; init; }
	public required IList<ILapidaryConverter>? UserDefinedConverters { get; internal set; }

	// TODO: Think harder about whether these are good Frozen candidates or you just wanted to use the cool new toy.
	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? ClassConverters { get; private set; }
	internal FrozenDictionary<Oop, ILapidaryConverter>? NumberConverters { get; private set; }
	internal FrozenDictionary<ConverterKey, ILapidaryConverter>? StructConverters { get; private set; }

	[MemberNotNullWhen(true, [nameof(ClassConverters), nameof(NumberConverters), nameof(StructConverters)])]
	public bool IsPrepared { get; private set; }

	public void PrepareDatabase(GemBuilderSession session)
	{
		// TODO: !!! Needs guard to stop concurrent initialisations.
		if (!IsPrepared)
		{
			ProcessConverters(session);
		}
	}

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
		if (IsPrepared)
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
		if (IsPrepared)
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

		UserDefinedConverters = null;
		IsPrepared = true;
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
}
