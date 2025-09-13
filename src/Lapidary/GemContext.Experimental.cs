using Lapidary.Experimental;
using System.Threading;
using System.Threading.Tasks;

namespace Lapidary;

public sealed partial class GemContext
{
	// TODO: Actually finish this...
	// TODO: Exception types here.

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
		try
		{
			// Break to cancel the call, then cleanup by "getting" the result.
			FFI.SoftBreak(Session);
			// TODO: Should have a no-wait FFI get call.
			_ = FFI.BlockForNonBlockingResult(Session);
			throw new InvalidOperationException("!! Async cancellation errored by something other than soft break.");
		}
		catch (GemException ex)
		{
			if (ex.Error.Number != 6003)
			{
				throw new InvalidOperationException("!! Async cancellation errored by something other than soft break.", ex);
			}
		}
	}

	internal bool ExecuteNonBlocking(ReadOnlySpan<byte> command)
	{
		return FFI.ExecuteNonBlocking(Session, command);
	}

	internal GemObject GetNonBlockingResult()
	{
		try
		{
			return new(Session, FFI.BlockForNonBlockingResult(Session));

		}
		catch (GemException ex)
		{
			if (ex.Error.Number is not 6003 and not 6004)
			{
				// TODO: Quick hack reference - 6003 = SoftBreak, 6004 HardBreak.
				// If it's neither of these, it's real and fatal.
				throw new InvalidOperationException("!! Async result after pooling killed by error.");
			}

			throw new InvalidOperationException("!! Async result after pooling killed by break.");
		}
	}

	internal void SoftBreak()
	{
		FFI.SoftBreak(Session);
	}
}
