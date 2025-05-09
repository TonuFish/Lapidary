﻿using System.Runtime.InteropServices;
using System.Threading;
using Lapidary.GemBuilder.Converters;

namespace Lapidary.GemBuilder.Core;

internal sealed class GemBuilderSession
{
    internal DatabaseBucket? Bucket { get; set; } // TODO: FFI Logins to return raw session, rather than this set.
    internal bool IsActiveSession => SessionId != GciSession.Zero;
    internal GciSession SessionId { get; init; } = GciSession.Zero;

    private readonly Queue<GemBuilderErrorInformation> _errors = new();

    public GemBuilderSession(GciSession sessionId)
    {
        SessionId = sessionId;
    }

    internal ILapidaryConverter? GetClassConverter(Type targetType, Oop classOop)
    {
        // TODO: Clarify null safety after restructure.
        return Bucket!.ClassConverters!.TryGetValue(new(classOop, targetType), out var converter)
            ? converter
            : null;
    }

    internal ILapidaryConverter? GetNumberConverter(Oop numberOop)
    {
        // TODO: Clarify null safety after restructure.
        return Bucket!.NumberConverters!.TryGetValue(numberOop, out var converter)
            ? converter
            : null;
    }

    internal ILapidaryConverter? GetStructConverter(Type targetType, Oop structOop)
    {
        // TODO: Clarify null safety after restructure.
        return Bucket!.StructConverters!.TryGetValue(new(structOop, targetType), out var converter)
            ? converter
            : null;
    }

    #region "Error Handling" (TEMPORARY CODE)

    private readonly Lock _errorLock = new();

    internal void AddError(ref GciErrSType error, [CallerMemberName] string source = "")
    {
        ReadOnlySpan<byte> messageBuffer;
        ReadOnlySpan<byte> reasonBuffer;
        Oop[]? argsBuffer = null;

        unsafe
        {
            fixed (GciErrSType* pin = &error)
            {
                messageBuffer = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(pin->message);
                reasonBuffer = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(pin->reason);

                if (error.argCount > 0)
                {
                    var rawArgs = new ReadOnlySpan<Oop>(pin->args, error.argCount);
                    argsBuffer = new Oop[error.argCount];
                    rawArgs.CopyTo(argsBuffer.AsSpan());
                }
            }
        }

        lock (_errorLock)
        {
            _errors.Enqueue(new(
                WhenUtc: DateTime.UtcNow,
                Source: source,
                Category: error.category,
                Context: error.context,
                ExceptionObj: error.exceptionObj,
                Args: argsBuffer,
                Number: error.number,
                ArgCount: error.argCount,
                Fatal: error.fatal,
                Message: messageBuffer.Length != 0 ? messageBuffer.DecodeUTF8() : null,
                Reason: reasonBuffer.Length != 0 ? reasonBuffer.DecodeUTF8() : null));
        }
    }

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
