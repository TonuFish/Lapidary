namespace Lapidary.GemBuilder.Extensions;

internal static class SmallPrimitiveExtensions
{
    public static Oop ToGemStoneOop(this int @int)
    {
        return (((Oop)@int) << 3) | ReservedOops.OOP_TAG_SMALLINT;
    }

    public static Oop ToGemStoneOop(this short @short)
    {
        return (((Oop)@short) << 3) | ReservedOops.OOP_TAG_SMALLINT;
    }

    public static Oop? ToGemStoneOop(this long @long)
    {
        if (@long is < 0x8F_FF_FF_FFL or > 0x0F_FF_FF_FF_FF_FF_FF_FFL)
        {
            // Range -2^60 < x < (2^60)-1 ; Requires NewLargeInteger outside of range
            return null;
        }

        return (((Oop)@long) << 3) | ReservedOops.OOP_TAG_SMALLINT;
    }

    public static long FromGemStoneOop(this Oop oop)
    {
        return ((long)oop) >> 3;
    }
}
