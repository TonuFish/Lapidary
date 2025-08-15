using Lapidary.GemBuilder.Definitions;

namespace Lapidary;

public sealed partial class GemContext
{
	internal GemBuilderSession Session { get; }

	#region Known Oops

	public GemObject FalseObject => new(Session, ReservedOops.OOP_FALSE);
	public GemObject NilObject => new(Session, ReservedOops.OOP_NIL);
	public GemObject TrueObject => new(Session, ReservedOops.OOP_TRUE);

	#endregion Known Oops

	internal GemContext(GemBuilderSession session)
	{
		Session = session;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GemObject Attach(PersistentGemObject gemObject)
	{
		ArgumentNullException.ThrowIfNull(gemObject);
		return new(Session, gemObject.Oop);
	}

	public GemObject PerformSmalltalkRaw(ReadOnlySpan<byte> command)
	{
		return new(Session, FFI.Execute(Session, command));
	}

	public bool TryGetClassObjectBySymbol(ReadOnlySpan<byte> symbol, out GemObject classObject)
	{
		// TODO: Consider ResolveSymbolObj instead - UTF8
		// TODO: Stackalloc or rent strategy
		Span<byte> symbolBuffer = stackalloc byte[symbol.Length + 1];
		symbolBuffer[^1] = 0;
		var classOop = FFI.ResolveSymbol(Session, symbolBuffer);

		classObject = new(Session, classOop);
		return classOop != ReservedOops.OOP_ILLEGAL;
	}

	#region Transactions

	public bool AbortTransaction()
	{
		return FFI.AbortTransaction(Session);
	}

	public bool BeginTransaction()
	{
		return FFI.BeginTransaction(Session);
	}

	public bool CommitTransaction()
	{
		return FFI.CommitTransaction(Session);
	}

	#endregion Transactions

	#region "Error Handling" (TEMPORARY CODE)

	public GemBuilderErrorInformation[] GetAllErrors()
	{
		return Session.GetAllErrors();
	}

	#endregion "Error Handling" (TEMPORARY CODE)
}
