namespace Lapidary.Authentication;

public readonly struct LoginIdentifier : IEquatable<LoginIdentifier>
{
	public string Id { get; }

	public static bool operator ==(LoginIdentifier left, LoginIdentifier right) => left.Equals(right);
	public static bool operator !=(LoginIdentifier left, LoginIdentifier right) => !(left == right);

	public LoginIdentifier(string id)
	{
		ArgumentNullException.ThrowIfNull(id);
		Id = id;
	}

	public bool Equals(LoginIdentifier other)
	{
		return Id.Equals(other.Id, StringComparison.Ordinal);
	}

	public override bool Equals(object? obj)
	{
		return obj is LoginIdentifier login && Equals(login);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode(StringComparison.Ordinal);
	}
}
