namespace Lapidary.Converters.Temporary;

internal sealed class IntegerConverter : LapidaryNumberConverter<long>
{
	public override IReadOnlyList<Oop> IdentifyingOops => [ReservedOops.OOP_CLASS_LargeInteger,];

	public override IReadOnlyList<string> IdentifyingSymbols => [];

	protected override ConversionResult<long> ConvertObject(GemObject gemObject)
	{
		return ConversionResult.FromResult(FFI.GetLargeInteger(gemObject.Session, gemObject.Oop));
	}
}
