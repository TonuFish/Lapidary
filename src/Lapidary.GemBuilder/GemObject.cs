using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Lapidary.GemBuilder;

#pragma warning disable CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
[DebuggerDisplay("Oop = {Oop}")]
public readonly ref partial struct GemObject : IEquatable<GemObject>
#pragma warning restore CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
{
	public readonly Oop Oop { get; init; }
	internal readonly GemBuilderSession Session { get; } // TODO: Don't really like this... Later.

	public readonly bool IsIllegalObject => Oop == ReservedOops.OOP_ILLEGAL;
	public readonly bool IsNilObject => Oop == ReservedOops.OOP_NIL;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal GemObject(GemBuilderSession session, Oop oop)
	{
		Oop = oop;
		Session = session;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private readonly GemObject IllegalObject()
	{
		return new(Session, ReservedOops.OOP_ILLEGAL);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly PersistentGemObject ToPersistentObject()
	{
		return new(Oop);
	}

	#region Info

	public readonly bool TryGetClassObject(out GemObject classObject)
	{
		if (FFI.TryGetObjectInfo(Session, Oop, out var info))
		{
			classObject = new(Session, info.objClass);
			return true;
		}

		classObject = IllegalObject();
		return false;
	}

	#endregion Info

	#region Foreign Perform

	public readonly GemObject Perform(ReadOnlySpan<byte> selector)
	{
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, ReadOnlySpan<Oop>.Empty));
	}

	public readonly GemObject Perform<T0>(
		ReadOnlySpan<byte> selector,
		T0 arg0)
		where T0 : allows ref struct
	{
		Span<Oop> args = [ConvertArgument(arg0)];
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform<T0, T1>(
		ReadOnlySpan<byte> selector,
		T0 arg0,
		T1 arg1)
		where T0 : allows ref struct
		where T1 : allows ref struct
	{
		Span<Oop> args = [ConvertArgument(arg0), ConvertArgument(arg1)];
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		Span<Oop> args = [ConvertArgument(arg0), ConvertArgument(arg1), ConvertArgument(arg2)];
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
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
		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
	}

	public readonly GemObject Perform(ReadOnlySpan<byte> selector, params ReadOnlySpan<object> argList)
	{
		// TODO: Fixed size stackalloc
		Span<Oop> args = stackalloc Oop[argList.Length];

		for (var ii = 0 ; ii < args.Length ; ++ii)
		{
			args[ii] = ConvertArgumentFromObject(argList[ii]);
		}

		return new(Session, FFI.ForeignPerform(Session, Oop, selector, args));
	}

	private readonly Oop ConvertArgument<TArg>(TArg argument) where TArg : allows ref struct
	{
		if (typeof(TArg) == typeof(GemObject))
		{
			return Unsafe.As<TArg, GemObject>(ref argument).Oop;
		}
		else if (typeof(TArg) == typeof(PersistentGemObject))
		{
			return Unsafe.As<TArg, PersistentGemObject>(ref argument).Oop;
		}
		else if (typeof(TArg) == typeof(string))
		{
			var text = Unsafe.As<TArg, string>(ref argument);

			scoped Span<byte> unicodeBytes;
			if (text is not null)
			{
				// TODO: This always sends as double byte
				var unicodeByteCount = Encoding.Unicode.GetByteCount(text);
				// TODO: Stackalloc or rent strategy
				unicodeBytes = stackalloc byte[unicodeByteCount];
				Encoding.Unicode.GetBytes(text, unicodeBytes);
			}
			else
			{
				unicodeBytes = Span<byte>.Empty;
			}

			var unicodeSpan = MemoryMarshal.Cast<byte, ushort>(unicodeBytes);
			return FFI.NewString(Session, (ReadOnlySpan<ushort>)unicodeSpan);
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
		else if (typeof(TArg).IsClass)
		{
			if (Unsafe.IsNullRef(ref argument))
			{
				return ReservedOops.OOP_NIL;
			}
			return ConvertArgumentReferenceOrBox(Unsafe.As<TArg, object>(ref argument));
		}

		return ThrowHelper.GenericExceptionToDetailLater<Oop>();
	}

	private readonly Oop ConvertArgumentFromObject(object argument)
	{
		if (argument is null)
		{
			return ReservedOops.OOP_NIL;
		}
		else if (argument is string text)
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

		return ConvertArgumentReferenceOrBox(argument);
	}

	private readonly Oop ConvertArgumentReferenceOrBox(object argument)
	{
		var type = argument.GetType();

		if (argument.GetType().IsClass)
		{
			// Callers guarantee supported classes have already been handled (string and PersistentGemObject)
			return ThrowHelper.GenericExceptionToDetailLater<Oop>();
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
		else
		{
			return ThrowHelper.GenericExceptionToDetailLater<Oop>();
		}
	}

	#endregion Foreign Perform

	#region Equality

	public static bool operator ==(GemObject left, GemObject right)
		=> left.Oop == right.Oop && left.Session == right.Session;

	public static bool operator !=(GemObject left, GemObject right) => !(left == right);

	public readonly bool Equals(GemObject other)
	{
		return Oop == other.Oop && Session == other.Session;
	}

	public readonly override int GetHashCode()
	{
		return Oop.GetHashCode();
	}

	#endregion Equality
}
