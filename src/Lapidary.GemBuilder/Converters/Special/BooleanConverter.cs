namespace Lapidary.GemBuilder.Converters.Special;

internal sealed class BooleanConverter : LapidaryStructConverter<bool>
{
    public override IList<Oop> IdentifyingOops => [ReservedOops.OOP_FALSE, ReservedOops.OOP_TRUE];

    public override IList<string> IdentifyingSymbols => [];

    protected override ConversionResult<bool> ConvertObject(GemObject gemObject)
    {
        return ConversionResult.FromResult(gemObject.Oop == ReservedOops.OOP_TRUE);
    }
}
