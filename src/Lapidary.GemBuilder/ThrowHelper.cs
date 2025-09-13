using Lapidary.GemBuilder.Extensions;
using System.Diagnostics;

namespace Lapidary.GemBuilder;

internal static class ThrowHelper
{
    // TODO: Real errors.

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static void GenericFFIException(ref GciErrSType error)
    {
        throw new GemException() { Error = error.WrapError() };
    }

    [DoesNotReturn, DebuggerHidden, StackTraceHidden]
    public static T GenericFFIException<T>(ref GciErrSType error)
    {
        throw new GemException() { Error = error.WrapError() };
    }
}
