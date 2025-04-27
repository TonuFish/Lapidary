using System.Buffers;
using System.Text;

namespace Lapidary.GemBuilder.Converters.Temporary;

internal sealed class OtherStringConverter : LapidaryClassConverter<string>
{
    public override IList<Oop> IdentifyingOops =>
        [
            ReservedOops.OOP_CLASS_DoubleByteString,
            ReservedOops.OOP_CLASS_DoubleByteSymbol,
            ReservedOops.OOP_CLASS_Unicode7,
            ReservedOops.OOP_CLASS_Unicode16,
            ReservedOops.OOP_CLASS_Unicode32,
        ];

    public override IList<string> IdentifyingSymbols => [];

    protected override ConversionResult<string> ConvertObject(GemObject gemObject)
    {
        // TODO: Replace the garbage method
        var bufferSize = ((int)FFI.GetObjectSize(gemObject.Session, gemObject.Oop) * 2) + 1;

        byte[]? rentedBuffer = null;
        scoped Span<byte> bytes;
        if (bufferSize > 256)
        {
            rentedBuffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            bytes = rentedBuffer.AsSpan();
        }
        else
        {
            bytes = stackalloc byte[bufferSize];
        }

        var populatedBytes = FFI.GetString(gemObject.Session, gemObject.Oop, bytes);

        var decodedString = !populatedBytes.IsEmpty ? Encoding.UTF8.GetString(populatedBytes) : string.Empty;

        if (rentedBuffer is not null)
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }

        return ConversionResult.FromResult(decodedString);
    }
}
