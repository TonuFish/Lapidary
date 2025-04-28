using Lapidary.GemBuilder.Experimental;
using System.Threading;
using System.Threading.Tasks;

namespace Lapidary.GemBuilder;

public sealed partial class GemContext
{
	public bool NonBlockingCallInProgress()
	{
		return FFI.PollForNonBlockingResult(Session) == false;
	}

	public Task<PersistentGemObject> PerformSmalltalkRawAsync(
		ReadOnlySpan<byte> command,
		CancellationToken ct = default)
	{
		// TODO: Inline NBE entirely once stable
		NonBlockingExecute nbe = new(this, ct);
		return nbe.StartAsync(command);
	}

	internal void ClearNonBlockingCall()
	{
		// Break to cancel the call, then cleanup by "getting" the result.
		FFI.SoftBreak(Session);
		// TODO: Should have a no-wait FFI get call.
		var oop = FFI.BlockForNonBlockingResult(Session);
		if (oop != ReservedOops.OOP_ILLEGAL
			|| !Session.TryGetError(out var error)
			|| error.Number != 6003)
		{
			throw new GemException("!! Async cancellation errored by something other than soft break.");
		}
	}

	internal bool ExecuteNonBlocking(ReadOnlySpan<byte> command)
	{
		return FFI.ExecuteNonBlocking(Session, command);
	}

	internal GemObject GetNonBlockingResult()
	{
		var oop = FFI.BlockForNonBlockingResult(Session);

		if (oop == ReservedOops.OOP_ILLEGAL)
		{
			_ = Session.TryGetError(out var error);

			if (error!.Number is not 6003 and not 6004)
			{
				// TODO: Quick hack reference - 6003 = SoftBreak, 6004 HardBreak.
				// If it's neither of these, it's real and fatal.
				throw new GemException("!! Async result after pooling killed by error.");
			}

			throw new GemException("!! Async result after pooling killed by break.");
		}

		return new(Session, oop);
	}

	internal void SoftBreak()
	{
		FFI.SoftBreak(Session);
	}
}
