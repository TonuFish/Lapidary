namespace Lapidary.GemBuilder;

public sealed class PersistentGemObject : IEquatable<PersistentGemObject>
{
    internal Oop Oop { get; init; }

    internal PersistentGemObject(Oop oop)
    {
        Oop = oop;
    }

    #region Equality

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is PersistentGemObject && Equals((PersistentGemObject)obj);
    }

    public bool Equals([NotNullWhen(true)] PersistentGemObject? other)
    {
        return other is not null && Oop == other.Oop;
    }

    public override int GetHashCode()
    {
        return Oop.GetHashCode();
    }

    #endregion Equality
}
