namespace Lapidary;

public readonly struct SessionIdentifier : IEquatable<SessionIdentifier>
{
    internal readonly Guid Id { private get; init; }

    internal SessionIdentifier(Guid id)
    {
        Id = id;
    }

    #region Equality

    public static bool operator ==(SessionIdentifier left, SessionIdentifier right) => left.Equals(right);

    public static bool operator !=(SessionIdentifier left, SessionIdentifier right) => !(left == right);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is SessionIdentifier && Equals((SessionIdentifier)obj);
    }

    public bool Equals(SessionIdentifier other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    #endregion Equality
}
