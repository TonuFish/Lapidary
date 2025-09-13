using System.Numerics;

namespace Lapidary.Converters;

public abstract class LapidaryClassConverter<T> : ILapidaryConverter where T : class
{
    public bool CanConvertToClass => true;

    public bool CanConvertToNumber => false;

    public bool CanConvertToStruct => false;

    public abstract IList<Oop> IdentifyingOops { get; }

    public abstract IList<string> IdentifyingSymbols { get; }

    public Type ConversionType => typeof(T);

    public TTo ConvertToClass<TTo>(GemObject gemObject) where TTo : class
    {
        var result = ConvertObject(gemObject);
        if (result.HasValue)
        {
            if (typeof(T).IsAssignableTo(typeof(TTo)))
            {
                return (TTo)(object)result._value;
            }

            return ThrowHelper.GenericExceptionToDetailLater<TTo>();
        }

        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    [DoesNotReturn]
    public TTo ConvertToNumber<TTo>(GemObject gemObject) where TTo : INumber<TTo>
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    [DoesNotReturn]
    public TTo ConvertToStruct<TTo>(GemObject gemObject) where TTo : struct
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    protected abstract ConversionResult<T> ConvertObject(GemObject gemObject);
}