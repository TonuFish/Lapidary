using System.Numerics;

namespace Lapidary.Converters;

public abstract class LapidaryStructConverter<T> : ILapidaryConverter where T : struct
{
    public bool CanConvertToClass => false;

    public bool CanConvertToNumber => false;

    public bool CanConvertToStruct => true;

    public abstract IList<Oop> IdentifyingOops { get; }

    public abstract IList<string> IdentifyingSymbols { get; }

    public Type ConversionType => typeof(T);

    [DoesNotReturn]
    public TTo ConvertToClass<TTo>(GemObject gemObject) where TTo : class
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    [DoesNotReturn]
    public TTo ConvertToNumber<TTo>(GemObject gemObject) where TTo : INumber<TTo>
    {
        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    public TTo ConvertToStruct<TTo>(GemObject gemObject) where TTo : struct
    {
        // TODO: Crap conversion is crap

        var result = ConvertObject(gemObject);
        if (result.HasValue && typeof(T) == typeof(TTo))
        {
            // The local here is really bad, but necessary with readonly ConversionResult
            // TODO: Consider dropping readonly on ConversionResult or having a mutable return for structs.
            var value = result._value;
            return Unsafe.As<T, TTo>(ref value);
        }

        return ThrowHelper.GenericExceptionToDetailLater<TTo>();
    }

    protected abstract ConversionResult<T> ConvertObject(GemObject gemObject);
}