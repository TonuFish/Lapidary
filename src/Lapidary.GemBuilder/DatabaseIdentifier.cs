namespace Lapidary.GemBuilder;

public readonly struct DatabaseIdentifier : IEquatable<DatabaseIdentifier>
{
    // TODO: No reason for this to be a struct, zzz.

    public required string Name { get; init; }

    [SetsRequiredMembers]
    public DatabaseIdentifier(string name)
    {
        Name = name;
    }

    #region Equality

    public static bool operator ==(DatabaseIdentifier left, DatabaseIdentifier right) => left.Equals(right);

    public static bool operator !=(DatabaseIdentifier left, DatabaseIdentifier right) => !(left == right);

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is DatabaseIdentifier && Equals((DatabaseIdentifier)obj);
    }

    public bool Equals(DatabaseIdentifier other)
    {
        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode(StringComparison.Ordinal);
    }

    #endregion Equality
}
