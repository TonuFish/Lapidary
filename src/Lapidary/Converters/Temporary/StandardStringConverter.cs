using System.Buffers;

namespace Lapidary.Converters.Temporary;

internal sealed class StandardStringConverter : LapidaryClassConverter<string>
{
    public override IList<Oop> IdentifyingOops => [ReservedOops.OOP_CLASS_STRING, ReservedOops.OOP_CLASS_SYMBOL];

    public override IList<string> IdentifyingSymbols => Array.Empty<string>();

    protected override ConversionResult<string> ConvertObject(GemObject gemObject)
    {
        // TODO: Replace the garbage method
        var bufferSize = (int)FFI.GetObjectSize(gemObject.Session, gemObject.Oop);
        var buffer = ArrayPool<byte>.Shared.Rent(minimumLength: bufferSize);
        var populatedBuffer = FFI.GetSingleByteString(gemObject.Session, gemObject.Oop, buffer.AsSpan());
        var utf8String = populatedBuffer.DecodeUTF8();
        ArrayPool<byte>.Shared.Return(buffer);

        return ConversionResult.FromResult(utf8String);
    }
}
