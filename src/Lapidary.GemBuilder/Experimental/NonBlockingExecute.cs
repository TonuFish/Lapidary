using System.Threading;
using System.Threading.Tasks;

namespace Lapidary.GemBuilder.Experimental;

public sealed class NonBlockingExecute
{
	private readonly GemContext _context;
	private readonly CancellationToken _ct;

	public NonBlockingExecute(GemContext context, CancellationToken ct = default)
	{
		_context = context;
		_ct = ct;
	}

	public Task<PersistentGemObject> StartAsync(ReadOnlySpan<byte> command)
	{
		return _context.ExecuteNonBlocking(command)
			? PollForResult()
			: Task.FromException<PersistentGemObject>(new GemException("Async call failed to start."));

		Task<PersistentGemObject> PollForResult()
		{
			var inProgress = false;
			do
			{
				inProgress = _context.NonBlockingCallInProgress();
			}
			while (inProgress && !_ct.IsCancellationRequested);

			if (_ct.IsCancellationRequested)
			{
				_context.ClearNonBlockingCall();
				return Task.FromCanceled<PersistentGemObject>(_ct);
			}

			// We trust another call hasn't started.
			var result = _context.GetNonBlockingResult();
			if (result.IsIllegalObject)
			{
				return Task.FromException<PersistentGemObject>(new GemException("Async call errored."));
			}

			return Task.FromResult<PersistentGemObject>(result.ToPersistentObject());
		}
	}

}
