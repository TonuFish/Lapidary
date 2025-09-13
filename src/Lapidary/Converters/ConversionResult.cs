namespace Lapidary.Converters;

public static class ConversionResult
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConversionResult<T> FromResult<T>(T value) where T : notnull
    {
        return new(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConversionResult<T> NoConversion<T>() where T : notnull
    {
        return new();
    }
}

public readonly struct ConversionResult<T> : IEquatable<ConversionResult<T>> where T : notnull
{
    public readonly bool HasValue { get; }

    internal readonly T _value;

    public ConversionResult()
    {
        HasValue = false;
        _value = default!;
    }

    internal ConversionResult(T value)
    {
        HasValue = true;
        _value = value;
    }

    #region Equality

    public static bool operator ==(ConversionResult<T> left, ConversionResult<T> right) => left.Equals(right);

    public static bool operator !=(ConversionResult<T> left, ConversionResult<T> right) => !(left == right);

    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ConversionResult<T> && Equals((ConversionResult<T>)obj);
    }

    public readonly bool Equals(ConversionResult<T> other)
    {
        return HasValue
            ? other.HasValue && _value.Equals(other._value)
            : !other.HasValue;
    }

    public override readonly int GetHashCode()
    {
        return HasValue ? _value.GetHashCode() : 0;
    }

    #endregion Equality
}
