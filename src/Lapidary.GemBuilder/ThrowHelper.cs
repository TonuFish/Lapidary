using System.Diagnostics;

namespace Lapidary.GemBuilder;

internal static class ThrowHelper
{
    // TODO: Real errors.

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static void GenericExceptionToDetailLater()
    {
        throw new GemException();
    }

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static T GenericExceptionToDetailLater<T>()
    {
        throw new GemException();
    }
}
