namespace Lapidary.Converters.Special;

internal sealed class BooleanConverter : LapidaryStructConverter<bool>
{
	public override IReadOnlyList<Oop> IdentifyingOops => [ReservedOops.OOP_FALSE, ReservedOops.OOP_TRUE,];

	public override IReadOnlyList<string> IdentifyingSymbols => [];

	protected override ConversionResult<bool> ConvertObject(GemObject gemObject)
	{
		return ConversionResult.FromResult(gemObject.Oop == ReservedOops.OOP_TRUE);
	}
}
