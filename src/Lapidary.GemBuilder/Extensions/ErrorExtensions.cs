using System.Runtime.InteropServices;

namespace Lapidary.GemBuilder.Extensions;

internal static class ErrorExtensions
{
	public static GemBuilderErrorInformation WrapError(this GciErrSType error, [CallerMemberName] string source = "")
	{
		ReadOnlySpan<byte> messageBuffer;
		ReadOnlySpan<byte> reasonBuffer;
		Oop[]? argsBuffer = null;

		unsafe
		{
			var pin = &error;

			messageBuffer = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(pin->message);
			reasonBuffer = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(pin->reason);

			if (error.argCount > 0)
			{
				var rawArgs = new ReadOnlySpan<Oop>(pin->args, error.argCount);
				argsBuffer = new Oop[error.argCount];
				rawArgs.CopyTo(argsBuffer.AsSpan());
			}
		}

		return new()
		{
			ArgCount = error.argCount,
			Args = argsBuffer,
			Category = error.category,
			Context = error.context,
			ExceptionObj = error.exceptionObj,
			Fatal = error.fatal,
			Message = messageBuffer.Length != 0 ? messageBuffer.DecodeUTF8() : null,
			Number = error.number,
			Reason = reasonBuffer.Length != 0 ? reasonBuffer.DecodeUTF8() : null,
			Source = source,
			WhenUtc = DateTime.UtcNow,
		};
	}
}
