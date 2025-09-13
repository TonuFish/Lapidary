using System.Numerics;

namespace Lapidary.Converters;

public interface ILapidaryConverter
{
    public bool CanConvertToClass { get; }

    public bool CanConvertToNumber { get; }

    public bool CanConvertToStruct { get; }

    public Type ConversionType { get; }

    public IList<Oop> IdentifyingOops { get; }

    public IList<string> IdentifyingSymbols { get; }

    public TTo ConvertToClass<TTo>(GemObject gemObject) where TTo : class;

    public TTo ConvertToNumber<TTo>(GemObject gemObject) where TTo : INumber<TTo>;

    public TTo ConvertToStruct<TTo>(GemObject gemObject) where TTo : struct;
}