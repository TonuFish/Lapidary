namespace Lapidary.GemBuilder.Converters.Temporary;

internal sealed class IntegerConverter : LapidaryNumberConverter<long>
{
    public override IList<Oop> IdentifyingOops => [ReservedOops.OOP_CLASS_LargeInteger];

    public override IList<string> IdentifyingSymbols => Array.Empty<string>();

    protected override ConversionResult<long> ConvertObject(GemObject gemObject)
    {
        return ConversionResult.FromResult(FFI.GetLargeInteger(gemObject.Session, gemObject.Oop));
    }
}
