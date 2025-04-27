namespace Lapidary.GemBuilder;

#pragma warning disable CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
public readonly ref partial struct GemObject
#pragma warning restore CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
{
    public bool GetDefinitelyBoolean()
    {
        return Oop switch
        {
            ReservedOops.OOP_TRUE => true,
            ReservedOops.OOP_FALSE => false,
            _ => ThrowHelper.GenericExceptionToDetailLater<bool>(),
        };
    }

    public string GetProbablySmallStandardString()
    {
        // TODO: "Standard" isn't exactly a clear term.
        // TODO: This bypasses any custom string handlers.

        if (GetConverterFromOop() is not null)
        {
            // TODO: Invariant string?
            ThrowHelper.GenericExceptionToDetailLater();
        }

        Span<byte> buffer = stackalloc byte[256];
        if (!FFI.TryGetObjectInfoWithStringBuffer(Session, Oop, out var info, buffer, out var stringBuffer))
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        if (!stringBuffer.IsEmpty && info.objClass is ReservedOops.OOP_CLASS_STRING or ReservedOops.OOP_CLASS_SYMBOL)
        {
            // Method's been used as intended.
            return stringBuffer.DecodeUTF8();
        }

        // Fallback to standard string handling.
        return GetString();
    }
}
