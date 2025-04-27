using System.Numerics;

namespace Lapidary.GemBuilder.Converters;

public abstract class LapidaryNumberConverter<T> : ILapidaryConverter where T : INumber<T>
{
    public bool CanConvertToClass => false;

    public bool CanConvertToNumber => true;

    public bool CanConvertToStruct => false;

    public abstract IList<Oop> IdentifyingOops { get; }

    public abstract IList<string> IdentifyingSymbols { get; }

    public Type ConversionType => typeof(T);

    [DoesNotReturn]
    public TTo ConvertToClass<TTo>(GemObject gemObject) where TTo : class
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    public TTo ConvertToNumber<TTo>(GemObject gemObject) where TTo : INumber<TTo>
    {
        var result = ConvertObject(gemObject);
        if (result.HasValue)
        {
            // TODO: Checked is very opinionated here, stew on it more.
            return TTo.CreateChecked(result._value);
        }

        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    [DoesNotReturn]
    public TTo ConvertToStruct<TTo>(GemObject gemObject) where TTo : struct
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    protected abstract ConversionResult<T> ConvertObject(GemObject gemObject);
}
