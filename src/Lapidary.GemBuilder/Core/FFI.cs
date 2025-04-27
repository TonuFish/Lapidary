namespace Lapidary.GemBuilder.Core;

internal static unsafe class FFI
{
    public static bool AbortTransaction(GemBuilderSession session)
    {
        GciErrSType error = new();

        var abortSuccessful = Methods.GciTsAbort(session.SessionId, ref error);
        if (abortSuccessful == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static bool BeginTransaction(GemBuilderSession session)
    {
        GciErrSType error = new();

        var openedTransaction = Methods.GciTsBegin(session.SessionId, ref error);
        if (openedTransaction == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static Oop BlockForNonBlockingResult(GemBuilderSession session)
    {
        GciErrSType error = new();

        var result = Methods.GciTsNbResult(session.SessionId, ref error);

        if (result == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
        }

        return result;
    }

    public static bool CommitTransaction(GemBuilderSession session)
    {
        GciErrSType error = new();

        var committedTransaction = Methods.GciTsCommit(session.SessionId, ref error);
        if (committedTransaction == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static bool ContinueProcessAfterException(GemBuilderSession session, Oop process)
    {
        GciErrSType error = new();

        var continueResult = Methods.GciTsContinueWith(
            session.SessionId,
            process,
            ReservedOops.OOP_ILLEGAL,
            null,
            0,
            ref error);

        if (continueResult == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static ReadOnlyMemory<byte>? Encrypt(ReadOnlySpan<byte> clearText)
    {
        // TODO: Clean and simplify the little experiment.

        if (clearText.Length == 0)
        {
            return null;
        }

        var size = clearText.Length + 24;
        while (EncryptNeedsLargerBuffer(clearText, size))
        {
            size++;
        }

        Memory<byte> encryptedBuffer = new(new byte[size]);
        var finalEncrypt = Methods.GciTsEncrypt(clearText, encryptedBuffer.Span, (size_t)encryptedBuffer.Length);
        if (finalEncrypt == null)
        {
            // This should never happen, but encrypt is dodgy as hell
            return null;
        }

        return encryptedBuffer;

        static bool EncryptNeedsLargerBuffer(ReadOnlySpan<byte> clearText, int size)
        {
            Span<byte> buffer = stackalloc byte[size];
            var retVal = Methods.GciTsEncrypt(clearText, buffer, (size_t)size);
            return retVal == null;
        }
    }

    public static Oop Execute(GemBuilderSession session, ReadOnlySpan<byte> command)
    {
        GciErrSType error = new();

        var oop = Methods.GciTsExecute(
            session.SessionId,
            command,
            ReservedOops.OOP_CLASS_Utf8,
            ReservedOops.OOP_ILLEGAL,
            ReservedOops.OOP_NIL,
            0,
            0,
            ref error);

        if (oop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return oop;
    }

    public static bool ExecuteNonBlocking(GemBuilderSession session, ReadOnlySpan<byte> command)
    {
        GciErrSType error = new();

        var result = Methods.GciTsNbExecute(
            session.SessionId,
            command,
            ReservedOops.OOP_CLASS_Utf8,
            ReservedOops.OOP_ILLEGAL,
            ReservedOops.OOP_NIL,
            0,
            0,
            ref error);

        if (result == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static Oop ForeignPerform(
        GemBuilderSession session,
        Oop root,
        ReadOnlySpan<byte> selector,
        ReadOnlySpan<Oop> args)
    {
        GciErrSType error = new();

        var oop = Methods.GciTsPerform(
            session.SessionId,
            root,
            ReservedOops.OOP_ILLEGAL,
            selector,
            args,
            args.Length,
            0,
            0,
            ref error);

        if (oop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return oop;
    }

    public static bool ForeignPerformNonBlocking(
        GemBuilderSession session,
        Oop root,
        ReadOnlySpan<byte> selector,
        ReadOnlySpan<Oop> args)
    {
        GciErrSType error = new();

        var result = Methods.GciTsNbPerform(
            session.SessionId,
            root,
            ReservedOops.OOP_ILLEGAL,
            selector,
            args,
            args.Length,
            0,
            0,
            ref error);

        if (result == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static int GetCollectionObjects(GemBuilderSession session, Oop root, long startIndex, Span<Oop> oops)
    {
        GciErrSType error = new();

        var oopCount = Methods.GciTsFetchOops(session.SessionId, root, startIndex, oops, oops.Length, ref error);
        if (oopCount == -1)
        {
            session.AddError(ref error);
            return 0;
        }

        return oopCount;
    }

    public static double GetFloat(GemBuilderSession session, Oop root)
    {
        GciErrSType error = new();
        Unsafe.SkipInit(out double @double);

        // Should only return false if `root` isn't a SmallDouble/Float
        var readSuccessful = Methods.GciTsOopToDouble(session.SessionId, root, ref @double, ref error);
        if (readSuccessful == 0)
        {
            session.AddError(ref error);
            return 0D;
        }

        return @double;
    }

    public static ReadOnlySpan<byte> GetGemBuilderVersion(Span<byte> buffer)
    {
        _ = Methods.GciTsVersion(buffer, (ulong)buffer.Length);
        return buffer[..(buffer.LastIndexOfAnyExcept((byte)0) + 1)];
    }

    public static long GetLargeInteger(GemBuilderSession session, Oop root)
    {
        GciErrSType error = new();
        Unsafe.SkipInit(out long @long);

        // Should only return false if the number exceeds the bounds of i64  (2^63-1, 2^-63)
        var readSuccessful = Methods.GciTsOopToI64(session.SessionId, root, ref @long, ref error);
        if (readSuccessful == 0)
        {
            session.AddError(ref error);
            return 0L;
        }

        return @long;
    }

    public static Oop GetObjectClass(GemBuilderSession session, Oop root)
    {
        GciErrSType error = new();

        var classOop = Methods.GciTsFetchClass(session.SessionId, root, ref error);
        if (classOop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return classOop;
    }

    public static long GetObjectSize(GemBuilderSession session, Oop root)
    {
        GciErrSType error = new();

        var size = Methods.GciTsFetchSize(session.SessionId, root, ref error);
        if (size == -1)
        {
            session.AddError(ref error);
            return 0L;
        }

        return size;
    }

    public static int GetSocket(GemBuilderSession session)
    {
        GciErrSType error = new();

        var result = Methods.GciTsSocket(session.SessionId, ref error);

        if (result == -1)
        {
            session.AddError(ref error);
        }

        return result;
    }

    public static ReadOnlySpan<byte> GetString(GemBuilderSession session, Oop root, Span<byte> buffer)
    {
        // NOTE -- As this is using GciTsFetchUtf8 instead of GciTsFetchUtf8Bytes, the string is null terminated
        // and the caller needs to add 1 to the buffer before passing it in.

        GciErrSType error = new();
        Unsafe.SkipInit(out long requiredSize);

        var bytesWritten = Methods
            .GciTsFetchUtf8(session.SessionId, root, buffer, buffer.Length, ref requiredSize, ref error);

        if (bytesWritten == -1)
        {
            session.AddError(ref error);
            return ReadOnlySpan<byte>.Empty;
        }

        return buffer[..(int)bytesWritten];
    }

    public static ReadOnlySpan<byte> GetSingleByteString(GemBuilderSession session, Oop root, Span<byte> buffer)
    {
        GciErrSType error = new();

        var bytesWritten = Methods
            .GciTsFetchBytes(session.SessionId, root, 1L, buffer, buffer.Length, ref error);

        if (bytesWritten == -1)
        {
            session.AddError(ref error);
            return ReadOnlySpan<byte>.Empty;
        }

        return buffer[..(int)bytesWritten];
    }

    public static void HardBreak(GemBuilderSession session)
    {
        GciErrSType error = new();

        var result = Methods.GciTsBreak(session.SessionId, 1, ref error);

        if (result == 0)
        {
            session.AddError(ref error);
        }
    }

    public static bool IsKindOfClass(GemBuilderSession session, Oop root, Oop @class)
    {
        GciErrSType error = new();

        var isClass = Methods.GciTsIsKindOfClass(session.SessionId, root, @class, ref error);
        if (isClass == -1)
        {
            session.AddError(ref error);
            return false;
        }

        return isClass == 1;
    }

    public static bool Login(
        ReadOnlySpan<byte> stoneName,
        ReadOnlySpan<byte> hostUsername,
        ReadOnlySpan<byte> hostPassword,
        ReadOnlySpan<byte> gemService,
        ReadOnlySpan<byte> username,
        ReadOnlySpan<byte> password,
        out GemBuilderSession session)
    {
        var sessionStarted = 0;
        GciErrSType error = new();

        var sessionId = Methods.GciTsLogin(
            StoneNameNrs: stoneName,
            HostUserId: hostUsername,
            HostPassword: hostPassword,
            hostPwIsEncrypted: 0,
            GemServiceNrs: gemService,
            gemstoneUsername: username,
            gemstonePassword: password,
            loginFlags: LoginFlags.GCI_LOGIN_QUIET,
            haltOnErrNum: 0,
            executedSessionInit: ref sessionStarted,
            err: ref error);

        // TODO: This has no business returning a full session object.
        session = new(sessionId);

        if (sessionId == 0 || sessionStarted == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static bool LoginEncrypted(
        ReadOnlySpan<byte> stoneName,
        ReadOnlySpan<byte> hostUsername,
        ReadOnlySpan<byte> hostPassword,
        ReadOnlySpan<byte> gemService,
        ReadOnlySpan<byte> username,
        ReadOnlySpan<byte> password,
        out GemBuilderSession session)
    {
        var sessionStarted = 0;
        GciErrSType error = new();

        var sessionId = Methods.GciTsLogin(
            StoneNameNrs: stoneName,
            HostUserId: hostUsername,
            HostPassword: hostPassword,
            hostPwIsEncrypted: 0,
            GemServiceNrs: gemService,
            gemstoneUsername: username,
            gemstonePassword: password,
            loginFlags: LoginFlags.GCI_LOGIN_PW_ENCRYPTED | LoginFlags.GCI_LOGIN_QUIET,
            haltOnErrNum: 0,
            executedSessionInit: ref sessionStarted,
                    err: ref error);

        // TODO: This has no business returning a full session object.
        session = new(sessionId);

        if (sessionId == 0 || sessionStarted == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static bool LoginX509(
        ReadOnlySpan<byte> netldiHostOrIp,
        ReadOnlySpan<byte> netldiNameOrPort,
        ReadOnlySpan<byte> privateKey,
        ReadOnlySpan<byte> cert,
        ReadOnlySpan<byte> caCert,
        ReadOnlySpan<byte> extraGemArgs,
        ReadOnlySpan<byte> dirArg,
        ReadOnlySpan<byte> logArg,
        bool argsArePemStrings, // TODO: Might be able to parse the args for this one
        out GemBuilderSession session)
    {
        var sessionStarted = 0;
        GciErrSType error = new();
        GciSession sessionId;

        fixed (byte* netldiHostOrIpPtr = &netldiHostOrIp.GetPinnableReference())
        fixed (byte* netldiNameOrPortPtr = &netldiNameOrPort.GetPinnableReference())
        fixed (byte* privateKeyPtr = &privateKey.GetPinnableReference())
        fixed (byte* certPtr = &cert.GetPinnableReference())
        fixed (byte* caCertPtr = &caCert.GetPinnableReference())
        fixed (byte* extraGemArgsPtr = &extraGemArgs.GetPinnableReference())
        fixed (byte* dirArgPtr = &dirArg.GetPinnableReference())
        fixed (byte* logArgPtr = &logArg.GetPinnableReference())
        {
            GciX509LoginArg loginArgs = new()
            {
                netldiHostOrIp = netldiHostOrIpPtr,
                netldiNameOrPort = netldiNameOrPortPtr,
                privateKey = privateKeyPtr,
                cert = certPtr,
                caCert = caCertPtr,
                extraGemArgs = extraGemArgsPtr,
                dirArg = dirArgPtr,
                logArg = logArgPtr,
                loginFlags = LoginFlags.GCI_LOGIN_QUIET,
                argsArePemStrings = argsArePemStrings ? 1 : 0,
                executedSessionInit = 0,
            };

            sessionId = Methods.GciTsX509Login(ref loginArgs, ref sessionStarted, ref error);
        }

        // TODO: This has no business returning a full session object.
        session = new(sessionId);

        if (sessionId == 0 || sessionStarted == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static void Logout(GemBuilderSession session)
    {
        GciErrSType error = new();

        if (Methods.GciTsLogout(session.SessionId, ref error) == 0)
        {
            session.AddError(ref error);
        }
    }

    public static Oop NewFloat(GemBuilderSession session, double @double)
    {
        GciErrSType error = new();

        var floatOop = Methods.GciTsDoubleToOop(session.SessionId, @double, ref error);
        if (floatOop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return floatOop;
    }

    public static Oop NewLargeInteger(GemBuilderSession session, long @long)
    {
        GciErrSType error = new();

        var longOop = Methods.GciTsI64ToOop(session.SessionId, @long, ref error);
        if (longOop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return longOop;
    }

    public static Oop NewSingleByteString(GemBuilderSession session, ReadOnlySpan<byte> bytes)
    {
        GciErrSType error = new();

        var stringOop = Methods.GciTsNewString_(session.SessionId, bytes, (ulong)bytes.Length, ref error);
        if (stringOop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return stringOop;
    }

    public static Oop NewString(GemBuilderSession session, ReadOnlySpan<ushort> bytes)
    {
        GciErrSType error = new();

        var stringOop = Methods.GciTsNewUnicodeString_(session.SessionId, bytes, (ulong)bytes.Length, ref error);
        if (stringOop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
            return ReservedOops.OOP_ILLEGAL;
        }

        return stringOop;
    }

    public static bool PersistObjects(GemBuilderSession session, ReadOnlySpan<Oop> oops)
    {
        GciErrSType error = new();

        var bufferEntirelyProcessed = Methods.GciTsSaveObjs(session.SessionId, oops, oops.Length, ref error);
        if (bufferEntirelyProcessed == 0)
        {
            session.AddError(ref error);
            return false;
        }

        return true;
    }

    public static bool? PollForNonBlockingResult(GemBuilderSession session)
    {
	    GciErrSType error = new();

	    var result = Methods.GciTsNbPoll(session.SessionId, 10, ref error);

	    if (result == -1)
	    {
		    session.AddError(ref error);
		    return null;
	    }

	    return result == 1;
    }

    public static Oop ResolveSymbol(GemBuilderSession session, ReadOnlySpan<byte> name)
    {
        GciErrSType error = new();

        var oop = Methods.GciTsResolveSymbol(session.SessionId, name, ReservedOops.OOP_NIL, ref error);
        if (oop == ReservedOops.OOP_ILLEGAL)
        {
            session.AddError(ref error);
        }

        return oop;
    }

    public static void SoftBreak(GemBuilderSession session)
    {
        GciErrSType error = new();

        var result = Methods.GciTsBreak(session.SessionId, 0, ref error);

        if (result == 0)
        {
            session.AddError(ref error);
        }
    }

    public static bool TryGetObjectInfo(GemBuilderSession session, Oop root, out GciTsObjInfo objectInfo)
    {
        GciErrSType error = new();
        GciTsObjInfo localObjInfo = new();

        var fetchSuccessful = Methods
            .GciTsFetchObjInfo(session.SessionId, root, 0, ref localObjInfo, null, 0, ref error);

        if (fetchSuccessful == -1)
        {
            session.AddError(ref error);
            objectInfo = default;
            return false;
        }

        objectInfo = localObjInfo;
        return true;
    }

    public static bool TryGetObjectInfoWithStringBuffer(
        GemBuilderSession session,
        Oop root,
        out GciTsObjInfo objectInfo,
        Span<byte> buffer,
        out ReadOnlySpan<byte> stringBuffer)
    {
        GciErrSType error = new();
        GciTsObjInfo localObjInfo = new();

        int64 bytesWritten;

        fixed (byte* bufferPtr = &buffer.GetPinnableReference())
        {
            bytesWritten = Methods.GciTsFetchObjInfo(
                session.SessionId,
                root,
                1,
                ref localObjInfo,
                bufferPtr,
                (size_t)buffer.Length,
                ref error);
        }

        if (bytesWritten == -1)
        {
            session.AddError(ref error);
            objectInfo = default;
            stringBuffer = ReadOnlySpan<byte>.Empty;
            return false;
        }

        objectInfo = localObjInfo;

        // If we have a string-like object that returns the representation we're looking to retrieve in `buffer`
        // AND it was written in full, make it available for the caller via `stringBuffer`.
        if (localObjInfo.objClass is ReservedOops.OOP_CLASS_STRING or ReservedOops.OOP_CLASS_SYMBOL)
        {
            stringBuffer = bytesWritten < buffer.Length
                ? (ReadOnlySpan<byte>)buffer[..(int)bytesWritten]
                : ReadOnlySpan<byte>.Empty;
        }
        else
        {
            stringBuffer = ReadOnlySpan<byte>.Empty;
        }

        return true;
    }
}
