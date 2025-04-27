using System.Numerics;

namespace Lapidary.GemBuilder.Converters.Special;

internal sealed class NilConverter : ILapidaryConverter
{
    public bool CanConvertToClass => false;

    public bool CanConvertToNumber => false;

    public bool CanConvertToStruct => false;

    public Type ConversionType => typeof(object);

    public IList<ulong> IdentifyingOops => [ReservedOops.OOP_NIL];

    public IList<string> IdentifyingSymbols => [];

    public TTo ConvertToClass<TTo>(GemObject gemObject) where TTo : class
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    public TTo ConvertToNumber<TTo>(GemObject gemObject) where TTo : INumber<TTo>
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    public TTo ConvertToStruct<TTo>(GemObject gemObject) where TTo : struct
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }
}
