namespace Lapidary.Converters.Special;

internal sealed class SmallDoubleConverter : LapidaryNumberConverter<double>
{
	public override IReadOnlyList<Oop> IdentifyingOops => [ReservedOops.OOP_TAG_SMALLDOUBLE,];

	public override IReadOnlyList<string> IdentifyingSymbols => [];

	protected override ConversionResult<double> ConvertObject(GemObject gemObject)
	{
		return ConversionResult.FromResult(FFI.GetFloat(gemObject.Session, gemObject.Oop));
	}
}
