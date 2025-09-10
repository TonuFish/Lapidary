using System.Diagnostics;

namespace Lapidary;

internal static class ThrowHelper
{
    // TODO: Real errors.

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static void GenericExceptionToDetailLater()
    {
        throw new InvalidOperationException();
    }

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static T GenericExceptionToDetailLater<T>()
    {
        throw new InvalidOperationException();
    }
}
