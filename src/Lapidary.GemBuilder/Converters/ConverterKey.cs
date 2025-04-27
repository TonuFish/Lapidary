namespace Lapidary.GemBuilder.Converters;

internal readonly struct ConverterKey : IEquatable<ConverterKey>
{
    private readonly Oop _oop;
    private readonly Type _type;

    public ConverterKey(Oop oop, Type type)
    {
        _oop = oop;
        _type = type;
    }

    #region Equality

    public static bool operator ==(ConverterKey left, ConverterKey right) => left.Equals(right);

    public static bool operator !=(ConverterKey left, ConverterKey right) => !(left == right);

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ConverterKey && Equals((ConverterKey)obj);
    }

    public bool Equals(ConverterKey other)
    {
        return _oop == other._oop && _type == other._type;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(_oop, _type);
    }

    #endregion Equality
}
