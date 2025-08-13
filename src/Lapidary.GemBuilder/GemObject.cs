using System.Diagnostics;

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
