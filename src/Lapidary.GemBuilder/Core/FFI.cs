namespace Lapidary.GemBuilder.Core;

public static class FFI
{
	public static bool AbortTransaction(GciSession session)
	{
		GciErrSType error = new();

		var abortSuccessful = Methods.GciTsAbort(session, ref error);
		if (abortSuccessful == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static bool BeginTransaction(GciSession session)
	{
		GciErrSType error = new();

		var openedTransaction = Methods.GciTsBegin(session, ref error);
		if (openedTransaction == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static Oop BlockForNonBlockingResult(GciSession session)
	{
		GciErrSType error = new();

		var result = Methods.GciTsNbResult(session, ref error);
		if (result == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return result;
	}

	public static bool CommitTransaction(GciSession session)
	{
		GciErrSType error = new();

		var committedTransaction = Methods.GciTsCommit(session, ref error);
		if (committedTransaction == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static bool ContinueProcessAfterException(GciSession session, Oop process)
	{
		GciErrSType error = new();

		Oop continueResult;
		unsafe
		{
			continueResult = Methods.GciTsContinueWith(
				session,
				process,
				ReservedOops.OOP_ILLEGAL,
				null,
				0,
				ref error);
		}
		if (continueResult == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
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

		unsafe
		{
			return Methods.GciTsEncrypt(clearText, encryptedBuffer.Span, (size_t)encryptedBuffer.Length) != null
				? encryptedBuffer
				: null;
		}

		static bool EncryptNeedsLargerBuffer(ReadOnlySpan<byte> clearText, int size)
		{
			Span<byte> buffer = stackalloc byte[size];
			unsafe
			{
				return Methods.GciTsEncrypt(clearText, buffer, (size_t)size) == null;
			}
		}
	}

	public static Oop Execute(GciSession session, ReadOnlySpan<byte> command)
	{
		GciErrSType error = new();

		var oop = Methods.GciTsExecute(
			session,
			command,
			ReservedOops.OOP_CLASS_Utf8,
			ReservedOops.OOP_ILLEGAL,
			ReservedOops.OOP_NIL,
			0,
			0,
			ref error);
		if (oop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return oop;
	}

	public static bool ExecuteNonBlocking(GciSession session, ReadOnlySpan<byte> command)
	{
		GciErrSType error = new();

		var result = Methods.GciTsNbExecute(
			session,
			command,
			ReservedOops.OOP_CLASS_Utf8,
			ReservedOops.OOP_ILLEGAL,
			ReservedOops.OOP_NIL,
			0,
			0,
			ref error);
		if (result == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static Oop Perform(
		GciSession session,
		Oop root,
		ReadOnlySpan<byte> selector,
		ReadOnlySpan<Oop> args)
	{
		GciErrSType error = new();

		var oop = Methods.GciTsPerform(
			session,
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
			ThrowHelper.GenericFFIException(ref error);
		}

		return oop;
	}

	public static bool PerformNonBlocking(
		GciSession session,
		Oop root,
		ReadOnlySpan<byte> selector,
		ReadOnlySpan<Oop> args)
	{
		GciErrSType error = new();

		var result = Methods.GciTsNbPerform(
			session,
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
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static int GetCollectionObjects(GciSession session, Oop root, long startIndex, Span<Oop> oops)
	{
		GciErrSType error = new();

		var oopCount = Methods.GciTsFetchOops(session, root, startIndex, oops, oops.Length, ref error);
		if (oopCount == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return oopCount;
	}

	public static double GetFloat(GciSession session, Oop root)
	{
		GciErrSType error = new();

		// Should only return false if `root` isn't a SmallDouble/Float
		double num = default;
		var readSuccessful = Methods.GciTsOopToDouble(session, root, ref num, ref error);
		if (readSuccessful == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return num;
	}

	public static ReadOnlySpan<byte> GetGemBuilderVersion(Span<byte> buffer)
	{
		_ = Methods.GciTsVersion(buffer, (ulong)buffer.Length);
		return buffer[..(buffer.LastIndexOfAnyExcept((byte)0) + 1)];
	}

	public static long GetLargeInteger(GciSession session, Oop root)
	{
		GciErrSType error = new();

		// Should only return false if the number exceeds the bounds of i64  (2^63-1, 2^-63)
		long num = default;
		var readSuccessful = Methods.GciTsOopToI64(session, root, ref num, ref error);
		if (readSuccessful == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return num;
	}

	public static Oop GetObjectClass(GciSession session, Oop root)
	{
		GciErrSType error = new();

		var classOop = Methods.GciTsFetchClass(session, root, ref error);
		if (classOop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return classOop;
	}

	public static long GetObjectSize(GciSession session, Oop root)
	{
		GciErrSType error = new();

		var size = Methods.GciTsFetchSize(session, root, ref error);
		if (size == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return size;
	}

	public static int GetSocket(GciSession session)
	{
		GciErrSType error = new();

		var result = Methods.GciTsSocket(session, ref error);
		if (result == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return result;
	}

	public static ReadOnlySpan<byte> GetString(GciSession session, Oop root, Span<byte> buffer)
	{
		// NOTE -- As this is using GciTsFetchUtf8 instead of GciTsFetchUtf8Bytes, the string is null terminated
		// and the caller needs to add 1 to the buffer before passing it in.

		GciErrSType error = new();

		long requiredSize = default;
		var bytesWritten = Methods.GciTsFetchUtf8(session, root, buffer, buffer.Length, ref requiredSize, ref error);
		if (bytesWritten == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return buffer[..(int)bytesWritten];
	}

	public static ReadOnlySpan<byte> GetSingleByteString(GciSession session, Oop root, Span<byte> buffer)
	{
		GciErrSType error = new();

		var bytesWritten = Methods.GciTsFetchBytes(session, root, 1L, buffer, buffer.Length, ref error);
		if (bytesWritten == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return buffer[..(int)bytesWritten];
	}

	public static void HardBreak(GciSession session)
	{
		GciErrSType error = new();

		var result = Methods.GciTsBreak(session, 1, ref error);
		if (result == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}
	}

	public static bool IsKindOfClass(GciSession session, Oop root, Oop @class)
	{
		GciErrSType error = new();

		var isClass = Methods.GciTsIsKindOfClass(session, root, @class, ref error);
		if (isClass == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return isClass == 1;
	}

	public static GciSession Login(
		ReadOnlySpan<byte> stoneName,
		ReadOnlySpan<byte> hostUsername,
		ReadOnlySpan<byte> hostPassword,
		ReadOnlySpan<byte> gemService,
		ReadOnlySpan<byte> username,
		ReadOnlySpan<byte> password)
	{
		GciErrSType error = new();

		var sessionStarted = 0;
		var session = Methods.GciTsLogin(
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

		if (session == 0 || sessionStarted == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return session;
	}

	public static GciSession LoginEncrypted(
		ReadOnlySpan<byte> stoneName,
		ReadOnlySpan<byte> hostUsername,
		ReadOnlySpan<byte> hostPassword,
		ReadOnlySpan<byte> gemService,
		ReadOnlySpan<byte> username,
		ReadOnlySpan<byte> password)
	{
		GciErrSType error = new();

		var sessionStarted = 0;
		var session = Methods.GciTsLogin(
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

		if (session == 0 || sessionStarted == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return session;
	}

	public static GciSession LoginX509(
		ReadOnlySpan<byte> netldiHostOrIp,
		ReadOnlySpan<byte> netldiNameOrPort,
		ReadOnlySpan<byte> privateKey,
		ReadOnlySpan<byte> cert,
		ReadOnlySpan<byte> caCert,
		ReadOnlySpan<byte> extraGemArgs,
		ReadOnlySpan<byte> dirArg,
		ReadOnlySpan<byte> logArg,
		bool argsArePemStrings // TODO: Might be able to parse the args for this one
		)
	{
		// TODO: Come back to this utter calamity.

		GciErrSType error = new();

		var sessionStarted = 0;
		GciSession session;
		unsafe
		{
#pragma warning disable RCS1001 // Add braces (when expression spans over multiple lines)
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

				session = Methods.GciTsX509Login(ref loginArgs, ref sessionStarted, ref error);
			}
#pragma warning restore RCS1001 // Add braces (when expression spans over multiple lines)
		}

		if (session == 0 || sessionStarted == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return session;
	}

	public static void Logout(GciSession session)
	{
		GciErrSType error = new();

		if (Methods.GciTsLogout(session, ref error) == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}
	}

	public static Oop NewFloat(GciSession session, double num)
	{
		GciErrSType error = new();

		var floatOop = Methods.GciTsDoubleToOop(session, num, ref error);
		if (floatOop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return floatOop;
	}

	public static Oop NewLargeInteger(GciSession session, long num)
	{
		GciErrSType error = new();

		var longOop = Methods.GciTsI64ToOop(session, num, ref error);
		if (longOop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return longOop;
	}

	public static Oop NewSingleByteString(GciSession session, ReadOnlySpan<byte> bytes)
	{
		GciErrSType error = new();

		var stringOop = Methods.GciTsNewString_(session, bytes, (ulong)bytes.Length, ref error);
		if (stringOop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return stringOop;
	}

	public static Oop NewString(GciSession session, ReadOnlySpan<ushort> bytes)
	{
		GciErrSType error = new();

		var stringOop = Methods.GciTsNewUnicodeString_(session, bytes, (ulong)bytes.Length, ref error);
		if (stringOop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return stringOop;
	}

	public static bool PersistObjects(GciSession session, ReadOnlySpan<Oop> oops)
	{
		GciErrSType error = new();

		var bufferEntirelyProcessed = Methods.GciTsSaveObjs(session, oops, oops.Length, ref error);
		if (bufferEntirelyProcessed == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return true;
	}

	public static bool? PollForNonBlockingResult(GciSession session)
	{
		GciErrSType error = new();

		var result = Methods.GciTsNbPoll(session, 10, ref error);
		if (result == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return result == 1;
	}

	public static Oop ResolveSymbol(GciSession session, ReadOnlySpan<byte> name)
	{
		GciErrSType error = new();

		var oop = Methods.GciTsResolveSymbol(session, name, ReservedOops.OOP_NIL, ref error);
		if (oop == ReservedOops.OOP_ILLEGAL)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		return oop;
	}

	public static void SoftBreak(GciSession session)
	{
		GciErrSType error = new();

		var result = Methods.GciTsBreak(session, 0, ref error);
		if (result == 0)
		{
			ThrowHelper.GenericFFIException(ref error);
		}
	}

	public static bool TryGetObjectInfo(GciSession session, Oop root, out GciTsObjInfo objectInfo)
	{
		GciErrSType error = new();

		GciTsObjInfo localObjInfo = new();
		int64 fetchSuccessful;
		unsafe
		{
			fetchSuccessful = Methods.GciTsFetchObjInfo(session, root, 0, ref localObjInfo, null, 0, ref error);
		}

		if (fetchSuccessful == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		objectInfo = localObjInfo;
		return true;
	}

	public static bool TryGetObjectInfoWithStringBuffer(
		GciSession session,
		Oop root,
		out GciTsObjInfo objectInfo,
		Span<byte> buffer,
		out ReadOnlySpan<byte> stringBuffer)
	{
		GciErrSType error = new();

		GciTsObjInfo localObjInfo = new();
		int64 bytesWritten;
		unsafe
		{
			fixed (byte* bufferPtr = &buffer.GetPinnableReference())
			{
				bytesWritten = Methods.GciTsFetchObjInfo(
					session,
					root,
					1,
					ref localObjInfo,
					bufferPtr,
					(size_t)buffer.Length,
					ref error);
			}
		}

		if (bytesWritten == -1)
		{
			ThrowHelper.GenericFFIException(ref error);
		}

		objectInfo = localObjInfo;

		// If we have a string-like object that returns the representation we're looking to retrieve in `buffer`
		// AND it was written in full, make it available for the caller via `stringBuffer`.
		if (localObjInfo.objClass is ReservedOops.OOP_CLASS_STRING or ReservedOops.OOP_CLASS_SYMBOL)
		{
			stringBuffer = bytesWritten < buffer.Length
				? (ReadOnlySpan<byte>)buffer[..(int)bytesWritten]
				: [];
		}
		else
		{
			stringBuffer = [];
		}

		return true;
	}
}
