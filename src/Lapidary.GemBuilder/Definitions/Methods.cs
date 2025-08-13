﻿using System.Runtime.InteropServices;

namespace Lapidary.GemBuilder.Definitions;

/// <summary>
/// Unix methods omitted.
/// </summary>
/// <remarks>
/// gcits.hf
/// </remarks>
internal static partial class Methods
{
	/// <summary>
	/// GciTsEncrypt
	/// Encrypts the clear text 'password'. Puts the encrypted form in
	/// outBuff and returns a pointer to the first character.
	/// Returns NULL if outBuff is not large enough, or password is NULL
	/// or an empty String.
	/// </summary>
	/// <remarks>
	/// <c>EXTERN_GCI_DEC(char*) GciTsEncrypt(const char* password, char* outBuf, size_t outBuffSize) GCI_WEAK;</c>
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static unsafe partial byte* GciTsEncrypt(
		/* const */ ReadOnlySpan<byte> password,
		Span<byte> outBuf,
		size_t outBuffSize);

	/// <summary>
	/// GciTsLogin
	/// Create a new session. A netldi is contacted per the NRS GemService, and
	/// optionally using HostUserId, HostPassword  to fork a gem process.
	/// Then the gem logs into the repository using StoneNameNrs, gemstoneUsername,
	/// gemstonePassword .
	/// Returns NULL if an error occurred, with details in *err .
	/// If result is non-NULL, login succeeded but there may still be a warning in
	/// *err .
	/// GCI_LOGIN_IS_SUBORDINATE bit in loginFlags is not allowed .
	/// "gcilnkobj" value for GemService is not allowed .
	/// GCI_LOGIN_PW_ENCRYPTED bit in loginFlags applies to gemstonePassword
	/// argument.
	/// haltOnErrNum, if non zero, specifes a value for GEM_HALT_ON_ERROR config
	/// parameter.
	/// Use GciTsEncrypt to encrypt passwords prior to calling GciTsLogin.
	/// If GemServiceNrs is NULL, the default "!@localhost!gemnetobject" is used.
	/// If StoneNameNrs is NULL, the default "gs64stone" is used
	/// HostUserId may be NULL if gem process is to run using the userId of the
	/// netldi  process .
	///
	/// Returns in *executedSessionInit TRUE if login succeeded
	/// and the VM executed  GsCurrentSession initialize .
	///
	/// Note, there are no equivalents to GciInit nor GciShutdown in the thread-safe
	/// GCI.
	/// The GciTsLogin function initializes all of the state related to the returned
	/// session and GciTsLogout will deallocate the specified session.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(GciSession) GciTsLogin(
	/// const char* StoneNameNrs,
	/// const char* HostUserId,
	/// const char* HostPassword,
	/// BoolType hostPwIsEncrypted,
	/// const char* GemServiceNrs,
	/// const char* gemstoneUsername,
	/// const char* gemstonePassword,
	/// unsigned int loginFlags /* per GCI_LOGIN* in gci.ht */ ,
	/// int haltOnErrNum,
	/// BoolType *executedSessionInit, /*output*/
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial GciSession GciTsLogin(
		/* const */ ReadOnlySpan<byte> StoneNameNrs,
		/* const */ ReadOnlySpan<byte> HostUserId,
		/* const */ ReadOnlySpan<byte> HostPassword,
		BoolType hostPwIsEncrypted,
		/* const */ ReadOnlySpan<byte> GemServiceNrs,
		/* const */ ReadOnlySpan<byte> gemstoneUsername,
		/* const */ ReadOnlySpan<byte> gemstonePassword,
		LoginFlags loginFlags,
		int haltOnErrNum,
		ref BoolType executedSessionInit,
		ref GciErrSType err);

	/// <summary>
	/// GciTsX509Login
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(GciSession)
	///	GciTsX509Login(GciX509LoginArg* args,
	/// BoolType* executedSessionInit,
	/// GciErrSType* err)  GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial GciSession GciTsX509Login(
		ref GciX509LoginArg args,
		ref BoolType executedSessionInit,
		ref GciErrSType err);

	/// <summary>
	/// GciTsLogout
	/// Logout the session.
	/// If the return value is FALSE, an error is returned in *err.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsLogout(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsLogout(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsNbLogout
	/// Logout the session. Do not wait for a result from the gem process .
	/// If the return value is FALSE, an error is returned in *err.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsNbLogout(GciSession sess,
	/// GciErrSType* err)  GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsNbLogout(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsSocket
	///
	/// Returns the file descriptor of the socket of the connection represented by "sess"
	/// Returns -1 if an error occurs or GciTsNbLogin is not complete
	/// The result is a file descriptor that can be used as argument to poll
	/// to determine if a GciTsNb operation has a result ready to read .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsSocket(GciSession sess, GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsSocket(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsSessionIsRemote
	/// Determinie if the given session is linked or RPC.
	/// -1 indicates session is not valid, 0 is linked, and 1 is RPC.
	/// Will not return 0 since linked sessions not currently supported
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsSessionIsRemote(GciSession sess) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsSessionIsRemote(GciSession sess);

	/// <summary>
	/// GciTsGemTrace
	/// For use in debugging the implementation.
	/// note that the printSendTrace() and printRecvTrace in linkgc.hc
	/// may be sufficient in a slow build without needing GciGemTrace(1)
	/// enable = 0 none, 1 commands, 2 commands+args , 3 even more
	/// Function result is previous value of the tracing state.
	/// Also enabled by    export GS_LGC_DEBUG=1   or
	/// export GS_LGC_DEBUG=2 in enviroments of libgcits.so and of netldid .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsGemTrace(GciSession sess,
	/// int enable,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsGemTrace(GciSession sess, int enable, ref GciErrSType err);

	/// <summary>
	/// GciTsResolveSymbol
	/// Lookup a C string in a SymbolList
	/// result OOP_ILLEGAL if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsResolveSymbol(GciSession sess,
	/// const char* str,
	/// OopType symbolList,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsResolveSymbol(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> str,
		OopType symbolList,
		ref GciErrSType err);

	/// <summary>
	/// GciTsResolveSymbolObj
	/// Lookup a Symbol object in a SymbolList
	/// result OOP_ILLEGAL if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsResolveSymbolObj(GciSession sess,
	/// OopType str,
	/// OopType symbolList,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsResolveSymbolObj(
		GciSession sess,
		OopType str,
		OopType symbolList,
		ref GciErrSType err);

	/// <summary>
	/// GciTsGetFreeOops
	///
	/// result is number of Oops in buf, or -1 if an error was return in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsGetFreeOops(GciSession sess,
	/// OopType* buf,
	/// int numOopsRequested,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsGetFreeOops(
		GciSession sess,
		Span<OopType> buf,
		int numOopsRequested,
		ref GciErrSType err);

	/// <summary>
	/// GciTsSaveObjs
	///
	/// result TRUE if buf completely processed, FALSE if error returned in *err
	/// since GciTs does not support user actions, this always adds the objects
	/// to the PureExportSet .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsSaveObjs(GciSession sess,
	/// OopType* buf,
	/// int count,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsSaveObjs(
		GciSession sess,
		ReadOnlySpan<OopType> buf,
		int count,
		ref GciErrSType err);

	/// <summary>
	/// GciTsReleaseObjs
	///
	/// result TRUE if buf completely processed, FALSE if error returned in *err
	/// the inverse of GciTsSaveObjs
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsReleaseObjs(GciSession sess,
	/// OopType* buf,
	/// int count,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsReleaseObjs(
		GciSession sess,
		ReadOnlySpan<OopType> buf,
		int count,
		ref GciErrSType err);

	/// <summary>
	/// GciTsReleaseAllObjs
	/// result TRUE if successful, FALSE if error returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsReleaseAllObjs(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsReleaseAllObjs(
		GciSession sess,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchBytes
	///
	/// function result is number of bytes returned in *dest or
	/// -1 if an error was returned in *err . *dest is undefined if
	/// result is -1.
	/// numBytes must be >= 0
	/// startIndex is one based (Smalltalk style)
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchBytes(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// ByteType* dest,
	/// int64 numBytes,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchBytes(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<ByteType> dest,
		int64 numBytes,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchChars
	///
	/// Returns -1 if an error returned in *err , in which case
	/// strlen(cString) == 0 .
	/// maxSize must be >= 1;
	/// startIndex is one based (Smalltalk style) .
	/// The bytes fetched are stored in memory
	/// starting at cString. At most maxSize - 1 bytes will be fetched from
	/// the object, and a \0 character will be stored in memory following
	/// the bytes fetched. The function returns the number of bytes fetched,
	/// excluding the null terminator character, which is equvalent to
	/// strlen(cString)
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchChars(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// char* cString,
	/// int64 maxSize,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchChars(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<byte> cString,
		int64 maxSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchUtf8Bytes
	///
	/// class of aString must be a identical to or a subclass of
	/// String, MultiByteString or Utf8 .
	/// If aString is an instance of Utf8 , or a kind of String with
	/// all codePoints &lt;= 127, *utf8String will be unchanged and behavior
	/// is the same as GciTsFetchBytes_ . Note that the result buffer
	/// contains bytes and may not start or end at codePoint boundaries
	/// within an instance of Utf8 .
	///
	/// If aString is a kind of String or MultiByteString with codePoints
	/// above 127,  and startIndex == 1 then
	/// aString is sent #encodeAsUTF8 and the result added to the export
	/// set and returned in *utf8String.
	/// Then bytes are fetched from *utf8String as for GciTsFetchBytes_ ,
	/// up to a maximum of bufSize.
	///
	/// startIndex represents a byte offset into the Utf8 encoded result.
	/// startIndex is one based (Smalltalk style) .
	/// If the function result == bufSize , then additional calls
	/// with startIndex values of  bufSize*1, bufSize*2,  etc are needed
	/// to obtain the complete result.
	///
	/// The caller should pass *utf8String to GciTsReleaseObjs
	/// after fetching all the bytes desired.
	///
	/// flags argument contains bits per GciFetchUtf8Flags in gci.ht
	///   0 = normal fetch
	///   GCI_UTF8_FilterIllegalCodePoints = substitute '.' for illegal codePoints
	///   GCI_UTF8_NoError = generate description in *buf instead of signalling an error
	///      when an illegal code point in aString prevents conversion to UTF8.
	///
	/// Returns -1 if an error was returned in *err , otherwise
	/// returns the number of bytes stored starting at *dest .
	/// There is no terminator zero included in *dest .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchUtf8Bytes(GciSession sess,
	/// OopType aString,
	/// int64 startIndex,
	/// ByteType *dest,
	/// int64 bufSize,
	/// OopType *utf8String,
	/// GciErrSType* err,
	/// int flags = 0) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchUtf8Bytes(
		GciSession sess,
		OopType aString,
		int64 startIndex,
		Span<ByteType> dest,
		int64 bufSize,
		ref OopType utf8String,
		ref GciErrSType err,
		GciFetchUtf8Flags flags = GciFetchUtf8Flags.GCI_UTF8_FetchNormal);

	/// <summary>
	/// GciTsStoreBytes
	///
	/// Returns FALSE if an error returned in *err .
	/// ofClass specifies the class which theObject is an instance of .
	/// For an object with multiple bytes per character or digit,
	/// "theBytes" is assumed to be in client native byte order,
	/// and will be swizzled if needed on the server. Also, startIndex
	/// and numBytes must be aligned on character/digit boundaries.
	///
	/// startIndex is one based (Smalltalk style)
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsStoreBytes(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// ByteType *theBytes,
	/// int64 numBytes,
	/// OopType ofClass,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsStoreBytes(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<ByteType> theBytes,
		int64 numBytes,
		OopType ofClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchOops
	///
	/// Returns -1 if an error returned in *err , otherwise returns the
	/// number of oops returned in *theOops , which will be &lt;= numOops .
	/// startIndex is one based (Smalltalk style)
	/// startIndex must be >= 1 .
	/// numOops must be >= 0 . theOops must be non-NULL if numOops > 0.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsFetchOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// OopType* theOops,
	/// int numOops,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsFetchOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	///  GciTsFetchNamedOops
	///
	///  Returns -1 if an error returned in *err , otherwise returns the
	///  number of oops returned in *theOops , which will be <= numOops .
	///  startIndex is one based (Smalltalk style)
	///  startIndex must be >= 1 .
	///  startIndex - 1 + numOops  must be <=  ( theObject class instSize )
	///  numOops must be >= 0 . theOops must be non-NULL if numOops > 0.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsFetchNamedOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// OopType *theOops,
	/// int numOops,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsFetchNamedOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	///  GciTsFetchVaryingOops
	///
	///  Returns -1 if an error returned in *err , otherwise returns the
	///  number of oops returned in *theOops , which will be <= numOops .
	///  startIndex is one based (Smalltalk style)
	///  startIndex must be >= 1 .
	///  theObject must be
	///  numOops must be >= 0 . theOops must be non-NULL if numOops > 0.
	///  Returns elements of an Nsc , as per    IdentityBag >> _at:
	///  or varying elements of an Array, as per   Array >> at:   .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsFetchVaryingOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// OopType *theOops,
	/// int numOops,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsFetchVaryingOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		Span<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	///  GciTsStoreOops
	///
	///  Returns FALSE if an error was returned in *err .
	///  startIndex must be >= 1 .
	///  If startIndex is within the named instVars of an object, stores into named instVars.
	///  If the oops to store extend beyond named instVars, stores into varying
	///  instvars of an oop format object, such as an Array .
	///  If overlay==TRUE,  theOops may contain elements with value OOP_ILLEGAL
	///  corresponding to instVars whose state will not be changed.
	///  Attempting to store into varying intVars of an nsc  ( theObject isKindOf: IdentityBag )
	///  will retun an error .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsStoreOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// const OopType* theOops,
	/// int numOops,
	/// GciErrSType *err,
	/// BoolType overlay = FALSE) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsStoreOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		/* const */ ReadOnlySpan<OopType> theOops,
		int numOops,
		ref GciErrSType err,
		BoolType overlay = 0);

	/// <summary>
	///  GciTsStoreNamedOops
	///  Returns FALSE if an error was returned in *err .
	///  startIndex must be >= 1 .
	///  startIndex - 1 + numOops  must be <=  ( theObject class instSize )
	///  If overlay==TRUE,  theOops may contain elements with value OOP_ILLEGAL
	///  corresponding to instVars whose state will not be changed.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsStoreNamedOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// const OopType *theOops,
	/// int numOops,
	/// GciErrSType *err,
	/// BoolType overlay = FALSE) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsStoreNamedOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		/* const */ ReadOnlySpan<OopType> theOops,
		int numOops,
		ref GciErrSType err,
		BoolType overlay = 0);

	/// <summary>
	///  GciTsStoreIdxOops
	///  Returns FALSE if an error was returned in *err .
	///  startIndex must be >= 1 .
	///  Stores into varying instVars of an oop format object, as per Array >> at:put:
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsStoreIdxOops(GciSession sess,
	/// OopType theObject,
	/// int64 startIndex,
	/// const OopType *theOops,
	/// int numOops,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsStoreIdxOops(
		GciSession sess,
		OopType theObject,
		int64 startIndex,
		/* const */ ReadOnlySpan<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	///  GciTsAddOopsToNsc
	///
	///  Returns FALSE if an error returned in *err,
	///  (theObject isKindOf: IdentityBag) must be true
	///  Adds objects to theObject, as per  IdentityBag >> addAll:
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsAddOopsToNsc(GciSession sess,
	/// OopType theObject,
	/// const OopType *theOops,
	/// int numOops,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsAddOopsToNsc(
		GciSession sess,
		OopType theObject,
		/* const */ ReadOnlySpan<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	/// GciTsRemoveOopsFromNsc
	///
	/// Returns -1 if an error returned in *err,
	/// 0 if any element of theOops was not present in theNsc,
	/// 1 if all elements of theOops were present in theNsc
	/// (theNsc isKindOf: IdentityBag) must be true .
	/// Removes objects from theNsc per   IdentityBag >> removeAll:
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsRemoveOopsFromNsc(GciSession sess,
	/// OopType theNsc,
	/// const OopType* theOops,
	/// int numOops,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsRemoveOopsFromNsc(
		GciSession sess,
		OopType theNsc,
		/* const */ Span<OopType> theOops,
		int numOops,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchObjInfo
	///
	/// Function result is >= 0 for success or
	/// -1 if an error other than read authorization failure was returned in *err .
	/// client side handling of special objects as before.
	/// addToExportSet has effect only if function result is 1
	/// If buffer not NULL, then up to bufSize bytes of the body of the object
	/// are returned in *buffer, and function result is the number of instVars returned.
	/// If buffer == NULL then function result is 0 for success or -1 for error.
	/// If read authorization is denied for objId, then result->access == 0 ,
	/// the rest of *result other than result->objId is zero , and function result is zero.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchObjInfo(GciSession sess,
	/// OopType objId,
	/// BoolType addToExportSet,
	/// GciTsObjInfo* result,
	/// ByteType* buffer,
	/// size_t bufSize,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static unsafe partial int64 GciTsFetchObjInfo(
		GciSession sess,
		OopType objId,
		BoolType addToExportSet,
		ref GciTsObjInfo result,
		ByteType* buffer,
		size_t bufSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchGbjInfo
	///  Function result is >= 0 for success or
	///  -2 object does not exist ,
	///  -1 if an error other than read authorization failure was returned in *err .
	///  client side handling of special objects as before.
	///  addToExportSet has effect only if function result is 1
	///  If buffer not NULL, then up to bufSize bytes of the body of the object
	///  are returned in *buffer, and function result is the number of instVars returned.
	///  If buffer == NULL then function result is 0 for success or -1 for error.
	///  If read authorization is denied for objId, then result->access == 0 ,
	///  the rest of *result other than result->objId is zero , and function result is zero.
	///
	///  GciTsGbjInfo includes additional information about the class of the object,
	///  to allow faster handling of varying kinds of results.
	///  See extraBits field of GciTsGbjInfo .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchGbjInfo(GciSession sess,
	/// OopType objId,
	/// BoolType addToExportSet,
	/// GciTsGbjInfo *result,
	/// ByteType *buffer,
	/// size_t bufSize,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static unsafe partial int64 GciTsFetchGbjInfo(
		GciSession sess,
		OopType objId,
		BoolType addToExportSet,
		ref GciTsGbjInfo result,
		ByteType* buffer,
		size_t bufSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchSize
	///
	/// Returns number of named plus varying instVars of obj ,
	/// or -1 if an error was returned in *err.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchSize(GciSession sess,
	/// OopType obj,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchSize(
		GciSession sess,
		OopType obj,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchVaryingSize
	///
	/// Returns number of varying instVars of obj ,
	/// or -1 if an error was returned in *err.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchVaryingSize(GciSession sess,
	/// OopType obj,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchVaryingSize(
		GciSession sess,
		OopType obj,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchClass
	///
	/// Returns the oop of the class of obj,
	/// or OOP_ILLEGAL if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsFetchClass(GciSession sess,
	/// OopType obj,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsFetchClass(
		GciSession sess,
		OopType obj,
		ref GciErrSType err);

	/// <summary>
	/// GciTsObjExists
	///
	/// Returns TRUE if session is valid and obj exists,  FALSE otherwise.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsObjExists(GciSession sess, OopType obj) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsObjExists(GciSession sess, OopType obj);

	/// <summary>
	/// GciTsIsKindOf
	///
	/// Equivalent to Object >> isKindOf:  where obj is the receiver
	/// and aClass is the argument .
	/// Returns -1 if an error was returned in *err,
	/// 0 for false result, 1 for true result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsIsKindOf(GciSession sess,
	/// OopType obj,
	/// OopType aClass,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsIsKindOf(
		GciSession sess,
		OopType obj,
		OopType aClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsIsSubclassOf
	///
	/// Equivalent to Behavior >> isSubclassOf: where cls is the receiver
	/// and aClass is the argument.
	/// Returns -1 if an error was returne in *err, 0 for false result, 1 for true
	/// result.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsIsSubclassOf(GciSession sess,
	/// OopType cls,
	/// OopType aClass,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsIsSubclassOf(
		GciSession sess,
		OopType cls,
		OopType aClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsIsKindOfClass
	///
	/// Equivalent to Object >> isKindOfClass:  where obj is the receiver
	/// and aClass is the argument.
	/// Returns -1 if an error occurs, 0 for false result, 1 for true result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsIsKindOfClass(GciSession sess,
	/// OopType obj,
	/// OopType aClass,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsIsKindOfClass(
		GciSession sess,
		OopType obj,
		OopType aClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsIsSubclassOfClass
	///
	/// Equivalent to Behavior >> _subclassOf: where cls is the receiver
	/// and aClass is the argument.
	/// Returns -1 if an error occurs, 0 for false result, 1 for true result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsIsSubclassOfClass(GciSession sess,
	/// OopType cls,
	/// OopType aClass,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsIsSubclassOfClass(
		GciSession sess,
		OopType cls,
		OopType aClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewObj
	///
	/// Creates an instance of aClass with varying size zero .
	/// Returns OOP_ILLEGAL if an error was returned in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewObj(GciSession sess,
	/// OopType aClass,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewObj(
		GciSession sess,
		OopType aClass,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewByteArray
	///
	/// Returns an an instance of ByteArray,
	/// or returns OOP_ILLEGAL if an error was returned in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewByteArray(GciSession sess,
	/// ByteType* body,
	/// size_t numBytes,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewByteArray(
		GciSession sess,
		Span<ByteType> body,
		size_t numBytes,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewString_
	/// Create a String object from a C string, specifying size.
	/// Returns an instance of String,
	/// or returns OOP_ILLEGAL if an error was returned in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewString_(GciSession sess,
	/// const char *cString,
	/// size_t nBytes,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewString_(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> cString,
		size_t nBytes,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewStringFromUtf16
	/// \*words  must be UTF16 encoded data
	/// returns OOP_ILLEGAL if an error occurred
	/// unicodeKind 0   create a String, DoubleByteString or QuadByteString
	///             1   create a Unicode7 , Unicode16 or Unicode32
	///            -1   create a string or unicode string per (Globals at:#StringConfiguration)
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewStringFromUtf16(GciSession sess,
	/// ushort *words,
	/// int64 nWords,
	/// int unicodeKind,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewStringFromUtf16(
		GciSession sess,
		Span<ushort> words, // TODO: double check this
		int64 nWords,
		int unicodeKind,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewString
	/// Create a String object from a null-terminated C string.
	/// Returns an instance of String,
	/// or returns OOP_ILLEGAL if an error was returned in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewString(GciSession sess,
	/// const char *cString,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewString(
		GciSession sess,
		/* const */ Span<byte> cString,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewSymbol
	/// Create a Symbol from a C string
	/// Returns an instance of Symbol ,
	/// or returns OOP_ILLEGAL if an error was returned in *err .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewSymbol(GciSession sess,
	/// const char *cString,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewSymbol(
		GciSession sess,
		/* const */ Span<byte> cString,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewUnicodeString_
	/// Create a UnicodeString object from UTF-16 encoded data.
	/// Returns OOP_ILLEGAL if an error was returned in *err .
	/// Result will be an instance of Unicode7, Unicode16, or Unicode32 .
	/// str must be legal UTF-16 encoded data .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewUnicodeString_(GciSession s,
	/// const ushort* str,
	/// size_t numShorts,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewUnicodeString_(
		GciSession s,
		/* const */ ReadOnlySpan<ushort> str,
		size_t numShorts,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewUnicodeString
	/// Create a UnicodeString object from UTF-16 encoded data.
	/// Returns OOP_ILLEGAL if an error was returned in *err .
	/// Result will be an instance of Unicode7, Unicode16, or Unicode32 .
	/// str must be legal UTF-16 encoded data.
	/// str is must be terminated by a codepoint of zero.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewUnicodeString(GciSession sess,
	/// const ushort* str,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewUnicodeString(
		GciSession sess,
		/* const */ ReadOnlySpan<ushort> str,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewUtf8String
	///
	/// Returns OOP_ILLEGAL if an error was returned in *err .
	/// utf8data must be legal UTF-8 data, terminated by a zero byte.
	/// If convertToUnicode==0, returns an instance of Utf8 .
	/// If convertToUnicode==1, returns an instance of
	/// Unicode7, Unicode16, or Unicode32 using the minimal
	/// character size required to represent utf8data.
	/// If convertToUnicode==1 and utf8data is 7 bit ascii,
	/// an instance of Unicode7 is returned.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewUtf8String(GciSession sess,
	/// const char* utf8data,
	/// BoolType convertToUnicode,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewUtf8String(
		GciSession sess,
		/* const */ Span<byte> utf8data,
		BoolType convertToUnicode,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNewUtf8String_
	///
	/// Returns OOP_ILLEGAL if an error was returned in *err .
	/// utf8data must contain nBytes bytes of legal UTF-8 data.
	/// If convertToUnicode==0, returns an instance of Utf8 .
	/// If convertToUnicode==1, returns an instance of
	/// Unicode7, Unicode16, or Unicode32 using the minimal
	/// character size required to represent utf8data.
	/// If convertToUnicode==1 and utf8data is 7 bit ascii,
	/// an instance of Unicode7 is returned
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNewUtf8String_(GciSession sess,
	/// const char* utf8data,
	/// size_t nBytes,
	/// BoolType convertToUnicode,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNewUtf8String_(
		GciSession sess,
		/* const */ Span<byte> utf8data,
		size_t nBytes,
		BoolType convertToUnicode,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchUnicode
	///
	/// class of obj must be a identical to or a subclass of
	/// String, DoubleByteString, or Unicode32 or Utf8 .
	///
	/// destSize is the size of the buffer *dest in shorts .
	///
	/// \*dest will be filled with UTF-16 encoded Characters .
	///
	/// Returns -1 if an error was returned in *err ,
	/// otherwise returns the number of codeunits (number of ushorts)
	/// that were stored at *dest.
	///
	/// Returns in *requiredSize the number of shorts required to hold
	/// the complete result including a terminator short of zero .
	/// If *requiredSize is > destSize, then *dest contains an incomplete
	/// result not terminated with a zero short.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchUnicode(GciSession sess,
	/// OopType obj,
	/// ushort* dest,
	/// int64 destSize,
	/// int64* requiredSize,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchUnicode(
		GciSession sess,
		OopType obj,
		Span<ushort> dest,
		int64 destSize,
		ref int64 requiredSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchUtf8
	///
	/// class of anObject must be a identical to or a subclass of
	/// String, DoubleByteString, or Unicode32 or Utf8 .
	///
	/// \*dest will be filled with UTF-8 encoded Characters .
	///
	/// Returns -1 if an error was returned in *err , otherwise
	/// returns the number of bytes stored starting at *dest,
	/// excluding the zero terminator byte.
	///
	/// Returns in *requiredSize the size required
	/// to hold the complete result, including a terminator byte of zero.
	///
	/// If *requiredSize is > destSize, then *dest contains an incomplete
	/// result not terminated with a zero byte, and *requiredSize is
	/// the worst case required size assumming all code points in anObject
	/// will produce a worst case encoding into a Utf8 .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsFetchUtf8(GciSession sess,
	/// OopType anObject,
	/// ByteType* dest,
	/// int64 destSize,
	/// int64* requiredSize,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsFetchUtf8(
		GciSession sess,
		OopType anObject,
		Span<byte> dest,
		int64 destSize,
		ref int64 requiredSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsClearStack
	///
	/// returns FALSE if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsClearStack(GciSession sess,
	/// OopType gsProcess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsClearStack(
		GciSession sess,
		OopType gsProcess,
		ref GciErrSType err);

	/// <summary>
	/// GciTsPerform
	///
	/// returns OOP_ILLEGAL if an error was returned in *err .
	/// Either selector == OOP_ILLEGAL and selectorStr is used
	/// or else selectorStr == NULL and selector is used.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsPerform(GciSession sess,
	/// OopType receiver,
	/// OopType aSymbol,
	/// const char* selectorStr,
	/// const OopType *args,
	/// int numArgs,
	/// int flags /* per GCI_PERFORM_FLAG* in gcicmn.ht */,
	/// ushort environmentId /* normally zero*/,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsPerform(
		GciSession sess,
		OopType receiver,
		OopType aSymbol,
		/* const */ ReadOnlySpan<byte> selectorStr,
		/* const */ ReadOnlySpan<OopType> args,
		int numArgs,
		int flags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNbPerform
	/// returns TRUE if execution started successfully .
	/// Use GciTsNbResult to get the result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsNbPerform(GciSession sess,
	/// OopType receiver,
	/// OopType aSymbol,
	/// const char* selectorStr,
	/// const OopType* args,
	/// int numArgs,
	/// int flags /* per GCI_PERFORM_FLAG* in gcicmn.ht */,
	/// ushort environmentId /* normally zero*/,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsNbPerform(
		GciSession sess,
		OopType receiver,
		OopType aSymbol,
		/* const */ ReadOnlySpan<byte> selectorStr,
		/* const */ ReadOnlySpan<OopType> args,
		int numArgs,
		int flags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNbResult
	/// returns OOP_ILLEGAL if an error occurred, with details in *err
	/// For use only after GciTsNbPerform or GsiTsNbExecute .
	/// This call does a blocking read on the socket of the GciSession.
	/// To avoid blocking, do a poll on the socket returned by GciTsSocket
	/// to determine if the result is ready to read .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsNbResult(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsNbResult(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsPerformFetchBytes
	///
	/// Send the selector specified by selectorStr to the specified receiver.
	/// If the result object is a byte format object,
	/// returns contents of the result object in *result,
	/// without any NUL byte termination.
	///
	/// Function result is the number of bytes returned in *result,
	/// or -1  if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(ssize_t) GciTsPerformFetchBytes(GciSession sess,
	/// OopType receiver,
	/// const char* selectorStr,
	/// const OopType *args,
	/// int numArgs,
	/// ByteType *result,
	/// ssize_t maxResultSize,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial ssize_t GciTsPerformFetchBytes(
		GciSession sess,
		OopType receiver,
		/* const */ ReadOnlySpan<byte> selectorStr,
		/* const */ ReadOnlySpan<OopType> args,
		int numArgs,
		Span<ByteType> result, // TODO: Wrapper must zero the alloc
		ssize_t maxResultSize,
		ref GciErrSType err);

	/// <summary>
	///  GciTsPerformFetchOops
	///
	///  Do a perform per  receiver, selector, args, num args.
	///    the result of which is expected to be an oop format object.
	///  Return up to maxResultSize of the instVars of the result of the perform.
	///  The result of the perform it is not added to the export set nor is it returned.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsPerformFetchOops(GciSession sess,
	/// OopType receiver,
	/// const char* selectorStr,
	/// const OopType *args,
	/// int numArgs,
	/// OopType *result,
	/// int maxResultSize,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsPerformFetchOops(
		GciSession sess,
		OopType receiver,
		/* const */ ReadOnlySpan<byte> selectorStr,
		/* const */ ReadOnlySpan<OopType> args,
		int numArgs,
		Span<OopType> result, // TODO: Wrapper must zero the alloc
		int maxResultSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsExecute
	///
	/// returns OOP_ILLEGAL if an error was returned in *err .
	/// If sourceStr is not NULL, it is used as the source string,
	/// and sourceOop specifies a class, typically OOP_CLASS_STRING or OOP_CLASS_Utf8,
	/// If sourceStr == NULL, then sourceOop is expected to be a kind of
	/// String, Unicode16, Unicode32, or Utf8 .
	/// If contextObj != OOP_ILLEGAL, source is compiled as if
	/// it were an instance method of the class of contextObj,
	/// otherwise compilation produces an anonymous method in which self == nil.
	/// If symbolList == OOP_NIL, use (System myUserProfile symbolList)
	/// to resolve literals in the compilation, otherwise use symbolList argument.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsExecute(GciSession sess,
	/// const char* sourceStr,
	/// OopType sourceOop,
	/// OopType contextObject,
	/// OopType symbolList,
	/// int flags /* per GCI_PERFORM_FLAG* in gcicmn.ht */,
	/// ushort environmentId /* normally zero*/,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsExecute(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> sourceStr,
		OopType sourceOop,
		OopType contextObject,
		OopType symbolList,
		int flags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNbExecute
	///
	/// Returns TRUE if execution started successfully.
	/// use GciTsNbResult to get the result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsNbExecute(GciSession sess,
	/// const char* sourceStr,
	/// OopType sourceOop,
	/// OopType contextObject,
	/// OopType symbolList,
	/// int flags /* per GCI_PERFORM_FLAG* in gcicmn.ht */,
	/// ushort environmentId /* normally zero*/,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsNbExecute(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> sourceStr,
		OopType sourceOop,
		OopType contextObject,
		OopType symbolList,
		int flags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsNbPoll
	///
	/// result
	///  1 - result or error is ready, call GciTsNbResult  to get the result or error .
	///  0 - result is not ready
	/// -1 - error, (invalid session, no NB call in progress, peer disconnected), details in gciErr.
	///
	///	timeoutMs values:
	/// 0 - do not block, return immediatly
	/// -1 - block forever until the Nb call finishes or an error occurs.
	/// > 0 block for up to timeoutMs ms before returning the result
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsNbPoll(GciSession session,
	/// int timeoutMs,
	/// GciErrSType *gciErr) GCI_WEAK ;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsNbPoll(GciSession session, int timeoutMs, ref GciErrSType gciErr);

	/// <summary>
	/// GciTsExecute_
	///
	/// variant of GciTsExecute .
	/// If sourceSize == -1, strlen(sourceStr) is used for the size of the source
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsExecute_(GciSession sess,
	/// const char* sourceStr,
	/// ssize_t sourceSize,
	/// OopType sourceOop,
	/// OopType contextObject,
	/// OopType symbolList,
	/// int flags /* per GCI_PERFORM_FLAG* in gcicmn.ht */,
	/// ushort environmentId /* normally zero*/,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsExecute_(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> sourceStr,
		ssize_t sourceSize,
		OopType sourceOop,
		OopType contextObject,
		OopType symbolList,
		int flags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsExecuteFetchBytes
	///
	/// variant of GciTsExecute_ which assumes that the execution result is
	/// a byte format object, usually an instance of String or Utf8 .
	/// The body of the result object is fetched into *result,
	/// and function result is number of bytes returned, or -1 if an error
	/// was returned in *err .
	/// Execution is in environment 0 using GCI_PERFORM_FLAG_ENABLE_DEBUG.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(ssize_t) GciTsExecuteFetchBytes(GciSession sess,
	/// const char* sourceStr,
	/// ssize_t sourceSize,
	/// OopType sourceOop,
	/// OopType contextObject,
	/// OopType symbolList,
	/// ByteType *result,
	/// ssize_t maxResultSize,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial ssize_t GciTsExecuteFetchBytes(
		GciSession sess,
		/* const */ ReadOnlySpan<byte> sourceStr,
		ssize_t sourceSize,
		OopType sourceOop,
		OopType contextObject,
		OopType symbolList,
		Span<ByteType> result,
		ssize_t maxResultSize,
		ref GciErrSType err);

	/// <summary>
	/// GciTsClassRemoveAllMethods
	///
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsClassRemoveAllMethods(GciSession sess,
	/// OopType aClass,
	/// ushort environmentId,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsClassRemoveAllMethods(
		GciSession sess,
		OopType aClass,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsProtectMethods
	///  if mode is TRUE, all subsequent method compilations in the
	///  current session will require a &lt;protected&gt; or &lt;unprotected&gt; token.
	///  until the function is called again with mode FALSE.
	///
	///  if mode is TRUE and the session is not logged in as SystemUser,
	///  the error RT_ERR_MUST_BE_SYSTEMUSER is generated, and no action is taken.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsProtectMethods(GciSession sess,
	/// BoolType mode,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsProtectMethods(
		GciSession sess,
		BoolType mode,
		ref GciErrSType err);

	/// <summary>
	/// GciTsCompileMethod
	/// Compile a method.
	/// Result is OOP_NIL if successful,
	///   OOP_ILLEGAL if an error was returned in *err,
	///   or oop of a String containing warnings.
	/// If category is OOP_NIL,  "as yet unclassified" is synthesized .
	/// If  symbolList is OOP_NIL,  (System myUserProfile symbolList)  is used.
	///
	/// overrideSelector  if not OOP_NIL, is OOP of a String which is
	///    converted to a Symbol and used in precedence to the selector pattern
	///    in the method source when installing the method in the method dictionary.
	///    Sending 'selector' to the resulting method will also reflect the
	///    overrideSelector argument.
	///    Values other than OOP_NIL are intended for bootstrapping Ruby image only.
	///
	/// compileFlags  zero or bits per GCI_COMPILE* in gcicmn.ht  .
	///              If bit GCI_COMPILE_CLASS_METH is one, compiles a class method,
	///              otherwise compiles an instance method.
	/// environmentId  a compilation environment identifier , normally zero .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsCompileMethod(GciSession sess,
	/// OopType source,
	/// OopType aClass,
	/// OopType category,
	/// OopType symbolList,
	/// OopType overrideSelector,
	/// int compileFlags,
	/// ushort environmentId,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsCompileMethod(
		GciSession sess,
		OopType source,
		OopType aClass,
		OopType category,
		OopType symbolList,
		OopType overrideSelector,
		int compileFlags,
		ushort environmentId,
		ref GciErrSType err);

	/// <summary>
	/// GciTsContinueWith
	///
	/// result is OOP_ILLEGAL if an error was returned in *err .
	///
	/// If continueWithError is not NULL, continue execution by signalling this
	/// error and replaceTopOfStack must be OOP_ILLEGAL . In this case,
	/// top frame of stack must be AbstractException>>signal
	/// or   AbstractException >>_signalFromPrimitive .
	///
	/// Within *continueWithError, if continueWithError->exceptionObj is not OOP_NIL
	/// it is used to replace self in the top frame,
	/// otherwise continueWithError->number is used to construct a kind of
	/// AbstractException to replace self in the top frame.
	/// Then execution is restarted at start of top frame's method.
	///
	/// replaceTopOfStack == OOP_ILLEGAL means TopOfStack will not be changed
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsContinueWith(GciSession sess,
	/// OopType gsProcess,
	/// OopType replaceTopOfStack,
	/// GciErrSType* continueWithError,
	/// int flags, /* same as GciPerformNoDebug flags,
	/// 	but single step has no effect */
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static unsafe partial OopType GciTsContinueWith(
		GciSession sess,
		OopType gsProcess,
		OopType replaceTopOfStack,
		GciErrSType* continueWithError,
		int flags,
		ref GciErrSType err);

	/// <summary>
	/// GciTsCallInProgress
	/// Returns 1 if a call is in progress on the specified session,
	/// 0 if a call is not in progress,
	/// -1 if sess is invalid in which case *err contains the details
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsCallInProgress(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsCallInProgress(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsAbort
	/// Abort the specified session.
	/// Implemented in client library as message send
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsAbort(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsAbort(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsBegin
	/// Begin a new transaction.
	/// Implemented in client library as message send
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsBegin(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsBegin(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsCommit
	/// Commit the specified session.
	/// Implemented in client library as message send
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsCommit(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsCommit(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsStoreTravDoTravRefs
	///
	/// Function result 0 = traversal completed,
	/// 1 = data returned but traversal not complete, -1 error in *err.
	/// If function result == 1, the application should call GciTsMoreTraversal
	/// to fetch the remainder of the traversal result.
	///
	/// This call  has several phases
	/// Phase 1
	/// Sends the oopsNoLongerReplicated, oopsGcedOnClient to the server,
	///   server removes oopsGcedOnClient from both PureExportSet and ReferencedSet,
	///     if an oop in oopsGcedOnClient is in neither PureExportSet nor
	///     ReferencedSet, or specifies a non-existant non-special object, no error
	///     is generated.
	///   server removes oopsNoLongerReplicated from the PureExportSet and
	///      adds them to the ReferencedSet .
	///     If any non-special object in oopsNoLongerReplicated
	///       was not actually in the PureExportSet, an error will be generated.
	///
	/// Phase 2, used the stdArgs argument and is equivalent to GciTsStoreTrav .
	/// stdArgs->storeTravBuff may be NULL if no store traversal phase
	/// is desired.
	///
	/// Note, stdArgs->alteredNumOops is ignored on input  .
	/// The output value of stdArgs->alteredNumOops is always zero,
	/// since altered objects are explicitly included in the traversal result.
	///
	/// Phase 3, the execution phase does one of
	/// GciPerformNoDebug, GciExecuteStrFromContextDbg, ExecuteBlock ,
	///   or no execution  per the stdArgs argument .
	///   See GciStoreTravDoArgsSType in gcicmn.ht for ExecuteBlock variant .
	///   This phase always uses GCI_PERFORM_RESULT_INTO_REFSET, to put
	///   non-special execution results into the ReferenceSet; the result may also
	///   be added to the PureExport set during the traversal phase below.
	///
	/// Phase 4 , does a special GciTsFetchTraversal . The root of this traversal will
	/// be two objects (one object if Phase 3 was "no execution")
	/// The first root object (-->Phase 4A) is an array of all of the objects that
	/// would have been returned from a GciAlteredObjs call after Phase 3.
	/// The second root object (--Phase 4B)  is the execution result from Phase 3 .
	/// Phases 4A, 4B use the same clamp spec specified by *ctArgs.
	/// Phase 4A operates with level == (ctArgs->level + 1) , Phase 4B operates
	/// with ctArgs->level .
	///
	/// Objects reported during Phase 4A are objects in the PureExportSet that
	/// were changed since the more recent of the last GciStoreTravDoTravRefs
	/// or the last time the dirty object sets were cleared, plus any child
	/// objects not already in the PureExportSet .
	///
	/// Objects reported during Phase 4B are those objects not already in
	/// PureExportSet
	///
	/// Execution result is not automatically put in PureExportSet; it is handled as
	///   per the traversal.
	/// Objects referenced by the execution result that are in the PureExportSet
	///   are not reported, however they are not clamped and traversal is continued
	///   through them to the level specified by the clamp specification.
	/// Objects for which the ctArgs cause a full report to be sent are added to the
	///   PureExportSet if not already in the PureExportSet .
	/// Objects for which the ctArgs would cause a header-only report to be sent,
	///      if an obj is in PureExportSet, report nothing
	///      else if obj not already in ReferencedSet, generate a header only report
	///            and add to ReferencedSet
	///
	/// All instVar values in object reports that are non-special and
	///   not otherwise reported and not in the PureExportSet
	///   are added to the ReferencedSet for both the
	///   altered objects and the execution result traversals.
	///
	/// The addSubleafHeaders instVar of the ctArgs->clampSpec object
	/// controls whether header-only reports are sent for instVar values
	/// at the bottom level of the traversal.
	///
	/// This function always sets GCI_TRAV_WITH_REF_SET bit in ctArgs->retrievalFlags.
	///
	/// The ReferencedSet protects its elements from GC ,
	/// but does not prevent committed objects from being faulted out of memory.
	/// There is no dirty tracking done on the ReferencedSet . The ReferencedSet
	/// represents those objects for which the client may have created a stub .
	///
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsStoreTravDoTravRefs(GciSession sess,
	/// const OopType* oopsNoLongerReplicated,
	/// int numNotReplicated,
	/// const OopType* oopsGcedOnClient,
	/// int numGced,
	/// GciStoreTravDoArgsSType *stdArgs,
	/// GciClampedTravArgsSType* ctArgs,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	[Obsolete("DO NOT USE - SUPPORTING STRUCTS AREN'T IMPLEMENTED YET", error: true)]
	public static partial int GciTsStoreTravDoTravRefs(
		GciSession sess,
		/* const */ ReadOnlySpan<OopType> oopsNoLongerReplicated,
		int numNotReplicated,
		/* const */ ReadOnlySpan<OopType> oopsGcedOnClient,
		int numGced,
		ref GciStoreTravDoArgsSType stdArgs,
		ref GciClampedTravArgsSType ctArgs,
		ref GciErrSType err);

	/// <summary>
	/// GciTsFetchTraversal
	///
	/// Performs a traversal starting at the oops specified by theOops and numOops,
	/// as specified by *ctArgs , returning object reports in ctArgs->travBuff.
	/// Returns 0 if traversal completed,
	/// 1 if data returned but traversal not complete,
	/// -1 if error returned in *err(in which case *travBuff undefined).
	/// If result == 1, call GciTsMoreTraversal again to fetch the remainder
	/// of the traversal result.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsFetchTraversal(GciSession sess,
	/// const OopType *theOops,
	/// int numOops,
	/// GciClampedTravArgsSType *ctArgs,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsFetchTraversal(
		GciSession sess,
		/* const */ ReadOnlySpan<OopType> theOops,
		int numOops,
		ref GciClampedTravArgsSType ctArgs,
		ref GciErrSType err);

	/// <summary>
	/// GciTsStoreTrav
	///
	/// result FALSE if error returned in *err, otherwise TRUE
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsStoreTrav(GciSession sess,
	/// GciTravBufType* travBuff,
	/// int flag,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsStoreTrav(
		GciSession sess,
		ref GciTravBufType travBuff,
		int flag,
		ref GciErrSType err);

	/// <summary>
	/// GciTsMoreTraversal
	///
	/// function result 1 if traversal completed,
	/// 0 if data returned but traversal not complete,
	/// -1 if an error was returned in *err (in which case *travBuff undefined)
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsMoreTraversal(GciSession sess,
	/// GciTravBufType *travBuff,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsMoreTraversal(
		GciSession sess,
		ref GciTravBufType travBuff,
		ref GciErrSType err);

	/// <summary>
	/// GciTsOopIsSpecial
	/// Return true if the object is a special, false otherwise.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsOopIsSpecial(OopType oop) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsOopIsSpecial(OopType oop);

	/// <summary>
	/// GciTsFetchSpecialClass
	/// Fetch the class of a special object.
	///  If oop is a legal special object, returns the object id of the class of oop,
	///  otherwise returns OOP_ILLEGAL.
	///  For a legal special object the result will be one of
	///    OOP_CLASS_SMALL_DOUBLE
	///    OOP_CLASS_SMALL_INTEGER
	///    OOP_CLASS_UNDEFINED_OBJECT
	///    OOP_CLASS_BOOLEAN
	///    OOP_CLASS_CHARACTER
	///    OOP_CLASS_JIS_CHARACTER
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType)  GciTsFetchSpecialClass(OopType oop) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsFetchSpecialClass(OopType oop);

	/// <summary>
	/// GciTsOopToChar
	///
	/// if oop is a legal instance of Character
	/// (i.e. if GciTsSpecialFetchClass(oop) == OOP_CLASS_CHARACTER)
	/// returns the code point of oop ,  otherwise returns -1.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsOopToChar(OopType oop) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsOopToChar(OopType oop);

	/// <summary>
	///  GciTsCharToOop
	///
	///  if  ch &lt;= 0x10ffff  returns the oop of the corresponding
	///  instance of Character, otherwise returns OOP_ILLEGAL
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsCharToOop(uint ch) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsCharToOop(uint ch);

	/// <summary>
	/// GciTsDirtyObjsInit
	///  Reinitializes the ExportedDirtyObjs set .
	///  Must be called before GciDirtySaveObjs to enable the collection of
	///  dirty objects.
	///
	/// Once this function has been called,
	/// the methods   System|commitTransaction
	///     and       System|abortTransaction
	/// will generate the error RT_ERR_COMMIT_ABORT_PENDING prior to the
	/// attempt to commit or prior to the abort, respectively.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsDirtyObjsInit(GciSession sess, GciErrSType *err)  GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsDirtyObjsInit(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsDoubleToSmallDouble
	///
	/// If the the argument is representable as a SmallDouble , return the oop
	/// representing that value , otherwise return OOP_ILLEGAL
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsDoubleToSmallDouble(double aFloat) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsDoubleToSmallDouble(double aFloat);

	/// <summary>
	/// GciTsDoubleToOop
	///
	///  Returns OOP_ILLEGAL if an error was returned in *err, otherwise
	///  returns the oop of a Float or SmallDouble representing aFloat.
	///  Will operate without a valid session if the result is a SmallDouble
	///  If aDouble is representable a SmallDouble, sess argument may be NULL .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsDoubleToOop(GciSession sess,
	/// double aDouble,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsDoubleToOop(
		GciSession sess,
		double aDouble,
		ref GciErrSType err);

	/// <summary>
	/// GciTsOopToDouble
	///
	/// Returns TRUE if oop is an instance of SmallDouble or Float
	/// in which case the numeric value is returned in *result.
	/// If oop is a SmallDouble, sess argument may be NULL .
	/// Otherwise returns FALSE .
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsOopToDouble(GciSession sess,
	/// OopType oop,
	/// double* result,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsOopToDouble(
		GciSession sess,
		OopType oop,
		ref double result,
		ref GciErrSType err);

	/// <summary>
	/// GciI32ToOop
	///
	/// Returns an instance of SmallInteger representing the arg.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciI32ToOop(int arg) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciI32ToOop(int arg);

	/// <summary>
	/// GciTsI32ToOop
	///
	/// Returns an instance of SmallInteger representing the arg.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsI32ToOop(int arg) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsI32ToOop(int arg);

	/// <summary>
	/// GciTsI64ToOop
	/// Returns OOP_ILLEGAL if an error occurred while creating a LargeInteger result.
	/// otherwise returns an instance of SmallInteger or LargeInteger representing arg.
	/// If arg is representable as a SmallInteger, sess argument may be NULL.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(OopType) GciTsI64ToOop(GciSession sess,
	/// int64 arg,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial OopType GciTsI64ToOop(
			GciSession sess,
			int64 arg,
			ref GciErrSType err);

	/// <summary>
	/// GciTsOopToI64
	///
	/// If oop is a SmallInteger, or if oop is an instance of LargeInteger
	/// within the range of a 64bit signed integer , returns the C integer in *result,
	/// and returns a function result of TRUE.
	/// If oop a SmallInteger, sess argument may be NULL.
	/// Otherwise returns FALSE and *result is undefined and an error is returne in *err.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsOopToI64(GciSession sess,
	/// OopType oop,
	/// int64 *result,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsOopToI64(
		GciSession sess,
		OopType oop,
		ref int64 result,
		ref GciErrSType err);

	/// <summary>
	/// GciTsBreak
	///
	/// Sends hard or soft break, returns FALSE if an error returned in *err .
	/// May be called while another C thread has a call in progress using
	/// the same GciSession. If no execution or traversal is in progress
	/// for the specified GciSession, has no effect.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsBreak(GciSession sess,
	/// BoolType hard,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsBreak(
		GciSession sess,
		BoolType hard,
		ref GciErrSType err);

	/// <summary>
	/// GciTsWaitForEvent
	///
	/// To be called from a separate application thread, this call blocks
	/// until an event is available or the session is shutdown by a logout or
	/// fatal error, or until GciTsCancelWaitForEvent is called.
	/// events are gem-gem signal, sigAbort, sigLostot, loss of session
	/// function result 1 , *evout has details , 0 no signal present,
	/// -1 error is in *err .
	/// A possible error is that some other thread is already waiting in
	/// GciTsWaitForEvent for the specified session.
	/// latencyMs defines how long this thread will sleep after the session
	/// received data from the gem before it polls again for an event.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciTsWaitForEvent(GciSession sess,
	/// int latencyMs,
	/// GciEventType *evout,
	/// GciErrSType *err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int GciTsWaitForEvent(
		GciSession sess,
		int latencyMs,
		ref GciEventType evout,
		ref GciErrSType err);

	/// <summary>
	/// GciTsCancelWaitForEvent
	///
	/// If result is FALSE, an error was returned in *err
	/// To be called from a thread other than the one waiting in GciTsWaitForEvent.
	/// causes any call to GciTsWaitForEvent to return.
	/// Has no effect if no thread is waiting in GciTsWaitForEvent for the specified
	/// session.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsCancelWaitForEvent(GciSession sess,
	/// GciErrSType* err) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsCancelWaitForEvent(GciSession sess, ref GciErrSType err);

	/// <summary>
	/// GciTsVersion
	///
	/// Can be called without a session.
	/// Function result is an integer indicating the GemStone product to which
	/// the client library belongs. GciTsVersion will always return 3.
	/// Defined integers are:
	///  1  GemStone/S
	///  2  GemStone/S 2G
	///  3  GemStone/S 64   (GciTsVersion returns this)
	///
	/// Returned in buf is a NUL terminated string that describes the GCI version.
	/// bufSize needs to be 128 or larger.
	/// Version fields in the string will be delimited by a '.'.
	/// The first field is the major version number,
	/// the second field is the minor version number.
	/// Any number of additional fields may exist. These
	/// additional fields will describe the exact release of the GCI.
	/// For additional version information use the methods in class System
	/// in the 'Version Management' category.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(uint) GciTsVersion(char *buf,
	/// size_t bufSize) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial uint GciTsVersion(Span<byte> buf, size_t bufSize);

	/// <summary>
	/// GciTsKeepAliveCount
	/// for use in testing the libgcits library.  returns number of keep alive bytes
	/// received by the session, or -1 if an error was returned in *err
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64) GciTsKeepAliveCount(GciSession s, GciErrSType *err) GCI_WEAK ;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsKeepAliveCount(GciSession s, ref GciErrSType err);

	/// <summary>
	/// GciTsDirtyExportedObjs
	/// This entry point returns a list of objects which are in the
	/// ExportedDirtyObjs, i.e. objects in the PureExportSet and whose state
	/// has been changed by one of the following:
	///     1.  Smalltalk execution,
	///     2.  Calls to GciStorePaths, GciSymDictAtObjPut, GciSymDictAtPut,
	///           GciStrKeyValueDictAtObjPut, GciStrKeyValueDictAtPut
	///     3.  Any GCI call from within a user action.
	///     4.  Committed by another transaction if a commit or abort
	///         was executed.
	///     5.  Aborted the modified state of a committed object.
	/// GciTsDirtyObjsInit() must be called once after login before
	/// GciTsDirtyExportedObjs() can be used.
	///
	/// Calls to GciTsStore*  functions do NOT put the modified object into
	/// the set of dirty objects.  The assumption is that the client does not
	/// want the dirty set to include modifications that the client has
	/// explicitly made.  EXCEPTION:  GciStore*, etc, calls from within
	/// a user action WILL put the modified object into the set of dirty
	/// objects.
	///
	/// The numOops argument is used as both an input and an output parameter.
	/// On input the value of numOops should be set to the maximum number of
	/// oops that can be returned in this call, i.e., the size (in oops)
	/// of the buffer specified by the first argument.  On output the numOops
	/// argument is set to the number of oops returned in the buffer.
	///
	/// The function result indicates whether the operation of returning
	/// the dirty objects is done.  If not done, i.e. FALSE, it is expected
	/// that the user will make repeated calls to this function until it
	/// returns a TRUE to indicate that all of the dirty objects have been
	/// returned.  If repeated calls are not made, then the unreturned objects
	/// will persist in the list until the function is next called.
	///
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciTsDirtyExportedObjs(GciSession s, OopType theOops[], int *numOops,
	/// GciErrSType *err) GCI_WEAK ;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciTsDirtyExportedObjs(
		GciSession s,
		Span<OopType> theOops,
		ref int numOops,
		ref GciErrSType err);

	/// <summary>
	/// GciTsKeyfilePermissions
	/// Returns -1 for error, or the uint32 value of keyfilePermissions from the stone process
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int64)
	/// GciTsKeyfilePermissions(GciSession s, GciErrSType *err)  GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial int64 GciTsKeyfilePermissions(GciSession s, ref GciErrSType err);

	/// <summary>
	/// GciUtf8To8bit
	///
	/// converts Utf8 input in *src to 8 bit data in *dest .
	/// If all code points in *src are in the range 0..255, and the
	/// result fits in destSize-1  , returns TRUE and *dest is null terminated,
	/// otherwise returns FALSE.
	/// Can be called without a session.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(BoolType) GciUtf8To8bit(const char* src,
	/// char *dest,
	/// ssize_t destSize) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial BoolType GciUtf8To8bit(
		/* const */ ReadOnlySpan<byte> src,
		Span<byte> dest,
		ssize_t destSize);

	/// <summary>
	/// GciNextUtf8Character
	/// For UTF-8 encoded src ,
	/// return the next legal UTF-8 code point in *chOut.
	/// Function result is the number of bytes in the that code point
	/// or -1 if the bytes are illegal for UTF-8 .
	/// Can be called without a session.
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(ssize_t) GciNextUtf8Character(const char* src,
	/// size_t len,
	/// uint *chOut) GCI_WEAK;
	/// </remarks>
	[LibraryImport(Version.GciTsDLL), DefaultDllImportSearchPaths(DllImportSearchPath.AssemblyDirectory)]
	public static partial ssize_t GciNextUtf8Character(
		/* const */ ReadOnlySpan<byte> src,
		size_t len,
		ref uint chOut);

	#region Unnecessary or Undocumented

	/*
	/// <summary>
	/// GciUnload
	/// Has no effect
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciUnload(void) GCI_WEAK;
	/// </remarks>
	public static partial void GciUnload();

	/// <summary>
	/// GciShutdown_
	/// Has no effect
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciShutdown() GCI_WEAK;
	/// </remarks>
	public static partial void GciShutdown();

	/// <summary>
	/// GciMalloc
	/// returns the result of calling malloc
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void*) GciMalloc(size_t length, int lineNum) GCI_WEAK;
	/// </remarks>
	public static partial void* GciMalloc(size_t length, int lineNum);

	/// <summary>
	/// <b>No documentation.</b>
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(int) GciHostCallDebuggerMsg(const char* msg)  GCI_WEAK;
	/// </remarks>
	public static partial int GciHostCallDebuggerMsg(/* const / byte* msg);

	/// <summary>
	/// GciFree
	///  returns the result of calling free
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciFree(void* ptr)  GCI_WEAK;
	/// </remarks>
	public static partial void GciFree(void* ptr);

	/// <summary>
	/// <b>No documentation.</b>
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciTimeStampMsStr(time_t seconds, unsigned short milliSeconds,
	/// char *result, size_t resultSize) GCI_WEAK;
	/// </remarks>
	public static partial void GciTimeStampMsStr(
		time_t seconds,
		ushort milliSeconds,
		char* result,
		size_t resultSize);

	/// <summary>
	/// <b>No documentation.</b>
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciHostFtime(time_t *sec, unsigned short *millitm) GCI_WEAK;
	/// </remarks>
	public static partial void GciHostFtime(time_t* sec, ushort* millitm);

	/// <summary>
	/// <b>No documentation.</b>
	/// </summary>
	/// <remarks>
	/// EXTERN_GCI_DEC(void) GciHostMilliSleep(unsigned int milliSeconds)  GCI_WEAK;
	/// </remarks>
	public static partial void GciHostMilliSleep(uint milliSeconds);
	*/

	#endregion Unnecessary or Undocumented
}
