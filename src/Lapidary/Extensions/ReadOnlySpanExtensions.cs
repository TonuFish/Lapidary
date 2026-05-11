using System.Buffers;
using System.Text;

namespace Lapidary.Extensions;

internal static class ReadOnlySpanExtensions
{
	extension(ReadOnlySpan<char> chars)
	{
		internal bool ContainsOnlyExtendedAscii()
		{
			return !chars.ContainsAnyExceptInRange(char.MinValue, (char)0xFF);
		}

		internal ReadOnlyMemory<byte> ToEncryptedNullTerminatedBytes()
		{
			// TODO: Double check \0.
			byte[]? rentedArray = null;
			var buffer = chars.Length <= 256
				? (stackalloc byte[256])[..chars.Length]
				: (rentedArray = ArrayPool<byte>.Shared.Rent(chars.Length)).AsSpan(0, chars.Length);

			_ = Encoding.UTF8.GetBytes(chars, buffer);

			var encryptedBuffer = FFI.Encrypt(buffer);
			if (!encryptedBuffer.HasValue)
			{
				ThrowHelper.GenericExceptionToDetailLater();
			}

			if (rentedArray is not null)
			{
				rentedArray.AsSpan().Clear();
				ArrayPool<byte>.Shared.Return(rentedArray);
			}

			return encryptedBuffer.Value;
		}

		internal ReadOnlyMemory<byte> ToNullTerminatedBytes()
		{
			var length = Encoding.UTF8.GetByteCount(chars);
			var buffer = new byte[length + 1];
			_ = Encoding.UTF8.GetBytes(chars, buffer);
			return buffer;
		}
	}
}
