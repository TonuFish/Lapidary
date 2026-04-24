using System.Threading;
using Lapidary.Converters;

namespace Lapidary.Core;

internal sealed class GemBuilderSession
{
	internal GemStoneState State { get; }
	internal GciSession Session { get; }

	private readonly Queue<GemBuilderErrorInformation> _errors = new();

	internal GemBuilderSession(GciSession session, GemStoneState state)
	{
		Session = session;
		State = state;
	}

	internal ILapidaryConverter? GetClassConverter(Type targetType, Oop classOop)
	{
		return State.ClassConverters.GetValueOrDefault(new(classOop, targetType));
	}

	internal ILapidaryConverter? GetNumberConverter(Oop numberOop)
	{
		return State.NumberConverters.GetValueOrDefault(numberOop);
	}

	internal ILapidaryConverter? GetStructConverter(Type targetType, Oop structOop)
	{
		return State.StructConverters.GetValueOrDefault(new(structOop, targetType));
	}

	#region VERY TEMPORARY IMPLICIT CONVERSION

	public static implicit operator GciSession(GemBuilderSession session) => session.Session;

	#endregion VERY TEMPORARY IMPLICIT CONVERSION

	#region "Error Handling" (TEMPORARY CODE)

	private readonly Lock _errorLock = new();

	internal bool TryGetError([NotNullWhen(true)] out GemBuilderErrorInformation? error)
	{
		bool hadError;
		GemBuilderErrorInformation? errorInformation;

		lock (_errorLock)
		{
			hadError = _errors.TryDequeue(out errorInformation);
		}

		if (hadError)
		{
			error = errorInformation!;
			return true;
		}

		error = null;
		return false;
	}

	internal GemBuilderErrorInformation[] GetAllErrors()
	{
		if (_errors.Count == 0)
		{
			return [];
		}

		lock (_errorLock)
		{
			var errors = _errors.ToArray();
			_errors.Clear();
			return errors;
		}
	}

	#endregion "Error Handling" (TEMPORARY CODE)
}
