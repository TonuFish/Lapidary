namespace Lapidary.GemBuilder.Converters.Special;

internal sealed class SmallDoubleConverter : LapidaryNumberConverter<double>
{
    public override IList<Oop> IdentifyingOops => [ReservedOops.OOP_TAG_SMALLDOUBLE];

    public override IList<string> IdentifyingSymbols => [];

    protected override ConversionResult<double> ConvertObject(GemObject gemObject)
    {
        return ConversionResult.FromResult(FFI.GetFloat(gemObject.Session, gemObject.Oop));
    }
}
