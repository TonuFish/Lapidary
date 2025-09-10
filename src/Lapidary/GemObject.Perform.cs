using System.Runtime.InteropServices;
using System.Text;

namespace Lapidary;
#pragma warning disable CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
public readonly ref partial struct GemObject
#pragma warning restore CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
{
	public readonly GemObject Perform(ReadOnlySpan<byte> selector)
	{
		return new(Session, FFI.Perform(Session, Oop, selector, ReadOnlySpan<Oop>.Empty));
	}

	public readonly GemObject Perform<T0>(
		ReadOnlySpan<byte> selector,
		T0 arg0)
		where T0 : allows ref struct
	{
		Span<Oop> args = [ConvertArgument(arg0),];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1)
		where T0 : allows ref struct
		where T1 : allows ref struct
	{
		Span<Oop> args = [ConvertArgument(arg0), ConvertArgument(arg1),];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
	{
		Span<Oop> args = [ConvertArgument(arg0), ConvertArgument(arg1), ConvertArgument(arg2),];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
		where T11 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
			ConvertArgument(arg11),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
		where T11 : allows ref struct
		where T12 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
			ConvertArgument(arg11),
			ConvertArgument(arg12),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
		where T11 : allows ref struct
		where T12 : allows ref struct
		where T13 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
			ConvertArgument(arg11),
			ConvertArgument(arg12),
			ConvertArgument(arg13),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
		where T11 : allows ref struct
		where T12 : allows ref struct
		where T13 : allows ref struct
		where T14 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
			ConvertArgument(arg11),
			ConvertArgument(arg12),
			ConvertArgument(arg13),
			ConvertArgument(arg14),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1,
		T2 arg2,
		T3 arg3,
		T4 arg4,
		T5 arg5,
		T6 arg6,
		T7 arg7,
		T8 arg8,
		T9 arg9,
		T10 arg10,
		T11 arg11,
		T12 arg12,
		T13 arg13,
		T14 arg14,
		T15 arg15)
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where T8 : allows ref struct
		where T9 : allows ref struct
		where T10 : allows ref struct
		where T11 : allows ref struct
		where T12 : allows ref struct
		where T13 : allows ref struct
		where T14 : allows ref struct
		where T15 : allows ref struct
	{
		Span<Oop> args =
		[
			ConvertArgument(arg0),
			ConvertArgument(arg1),
			ConvertArgument(arg2),
			ConvertArgument(arg3),
			ConvertArgument(arg4),
			ConvertArgument(arg5),
			ConvertArgument(arg6),
			ConvertArgument(arg7),
			ConvertArgument(arg8),
			ConvertArgument(arg9),
			ConvertArgument(arg10),
			ConvertArgument(arg11),
			ConvertArgument(arg12),
			ConvertArgument(arg13),
			ConvertArgument(arg14),
			ConvertArgument(arg15),
		];
		return new(Session, FFI.Perform(Session, Oop, selector, args));
	}

	private readonly Oop ConvertArgument<TArg>(TArg argument) where TArg : allows ref struct
	{
		if (typeof(TArg) == typeof(GemObject))
		{
			return Unsafe.As<TArg, GemObject>(ref argument).Oop;
		}
		else if (typeof(TArg) == typeof(int))
		{
			return Unsafe.As<TArg, int>(ref argument).ToGemStoneOop();
		}
		else if (typeof(TArg) == typeof(double))
		{
			return FFI.NewFloat(Session, Unsafe.As<TArg, double>(ref argument));
		}
		else if (typeof(TArg) == typeof(long))
		{
			var num = Unsafe.As<TArg, long>(ref argument);
			return num.ToGemStoneOop() ?? FFI.NewLargeInteger(Session, num);
		}
		else if (typeof(TArg) == typeof(bool))
		{
			return Unsafe.As<TArg, bool>(ref argument) ? ReservedOops.OOP_TRUE : ReservedOops.OOP_FALSE;
		}
		else if (typeof(TArg) == typeof(short))
		{
			return Unsafe.As<TArg, short>(ref argument).ToGemStoneOop();
		}
		else if (typeof(TArg) == typeof(float))
		{
			var num = (double)Unsafe.As<TArg, float>(ref argument);
			return FFI.NewFloat(Session, num);
		}
		// TODO: Measure effect of string and persistentGO test here - Very likely worth to re-introduce
		else if (typeof(TArg).IsClass)
		{
			return Unsafe.IsNullRef(in argument)
				? ReservedOops.OOP_NIL
				: ConvertArgumentNotNullReferenceOrBox(Unsafe.As<TArg, object>(ref argument));
		}

		return ThrowHelper.GenericExceptionToDetailLater<Oop>();
	}

	private readonly Oop ConvertArgumentFromObject(object argument)
	{
		return (argument is null) ? ReservedOops.OOP_NIL : ConvertArgumentNotNullReferenceOrBox(argument);
	}

	private readonly Oop ConvertArgumentNotNullReferenceOrBox(object argument)
	{
		var type = argument.GetType();

		if (type.IsClass)
		{
			return ConvertArgumentNotNullReference(argument);
		}
		else if (type == typeof(int))
		{
			return ((int)argument).ToGemStoneOop();
		}
		else if (type == typeof(double))
		{
			return FFI.NewFloat(Session, (double)argument);
		}
		else if (type == typeof(long))
		{
			var num = (long)argument;
			return num.ToGemStoneOop() ?? FFI.NewLargeInteger(Session, num);
		}
		else if (type == typeof(bool))
		{
			return (bool)argument ? ReservedOops.OOP_TRUE : ReservedOops.OOP_FALSE;
		}
		else if (type == typeof(short))
		{
			return ((short)argument).ToGemStoneOop();
		}
		else if (type == typeof(float))
		{
			var num = (double)(float)argument;
			return FFI.NewFloat(Session, num);
		}

		return ThrowHelper.GenericExceptionToDetailLater<Oop>();
	}

	private readonly Oop ConvertArgumentNotNullReference(object argument)
	{
		// This method would be local to ConvertArgumentNotNullReferenceOrBox if this wasn't a struct.

		if (argument is string text)
		{
			// TODO: This always sends as double byte
			var unicodeByteCount = Encoding.Unicode.GetByteCount(text);
			// TODO: Stackalloc or rent strategy
			Span<byte> unicodeBytes = stackalloc byte[unicodeByteCount];
			Encoding.Unicode.GetBytes(text, unicodeBytes);

			var unicodeSpan = MemoryMarshal.Cast<byte, ushort>(unicodeBytes);
			return FFI.NewString(Session, (ReadOnlySpan<ushort>)unicodeSpan);
		}
		else if (argument is PersistentGemObject obj)
		{
			return obj.Oop;
		}

		return ThrowHelper.GenericExceptionToDetailLater<Oop>();
	}
}
