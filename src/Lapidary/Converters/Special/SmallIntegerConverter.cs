namespace Lapidary.Converters.Special;

internal sealed class SmallIntegerConverter : LapidaryNumberConverter<long>
{
	public override IReadOnlyList<Oop> IdentifyingOops => [ReservedOops.OOP_TAG_SMALLINT,];

	public override IReadOnlyList<string> IdentifyingSymbols => [];

	protected override ConversionResult<long> ConvertObject(GemObject gemObject)
	{
		// TODO: Do SmallInteger re-arranging
		return ConversionResult.FromResult(FFI.GetLargeInteger(gemObject.Session, gemObject.Oop));
	}
}
