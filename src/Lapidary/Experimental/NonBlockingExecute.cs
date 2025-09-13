using System.Threading;
using System.Threading.Tasks;

namespace Lapidary.Experimental;

public sealed class NonBlockingExecute
{
	private readonly GemContext _context;
	private readonly CancellationToken _ct;

	public NonBlockingExecute(GemContext context, CancellationToken ct = default)
	{
		// TODO: Exception types here.
		_context = context;
		_ct = ct;
	}

	public Task<PersistentGemObject> StartAsync(ReadOnlySpan<byte> command)
	{
		return _context.ExecuteNonBlocking(command)
			? PollForResult()
			: Task.FromException<PersistentGemObject>(new InvalidOperationException("Async call failed to start."));

		Task<PersistentGemObject> PollForResult()
		{
			bool inProgress;
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
				return Task.FromException<PersistentGemObject>(new InvalidOperationException("Async call errored."));
			}

			return Task.FromResult<PersistentGemObject>(result.ToPersistentObject());
		}
	}
}
