namespace Lapidary.Converters.Temporary;

internal sealed class FloatConverter : LapidaryNumberConverter<double>
{
    public override IList<Oop> IdentifyingOops =>
        [
            ReservedOops.OOP_CLASS_Float,
            ReservedOops.OOP_CLASS_BINARY_FLOAT,
            ReservedOops.OOP_CLASS_SmallFloat,
        ];

    public override IList<string> IdentifyingSymbols => [];

    protected override ConversionResult<double> ConvertObject(GemObject gemObject)
    {
        return ConversionResult.FromResult(FFI.GetFloat(gemObject.Session, gemObject.Oop));
    }
}
