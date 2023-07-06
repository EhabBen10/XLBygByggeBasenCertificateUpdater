﻿namespace CertificateUpdater.Domain.Shared;
public class Error : IEquatable<Error>
{
	public static readonly Error None = new(string.Empty, string.Empty);

	public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

	public static readonly Error NotImplemented = new("Error.NotImplemented", "Called method is not implemented");

	public Error(string code, string message)
	{
		Code = code;
		Message = message;
	}

	public string Code { get; }

	public string Message { get; }

	public static implicit operator string(Error error) => error.Code;

	public static bool operator ==(Error? a, Error? b)
	{
		if (a is null && b is null) return true;
		if (a is null || b is null) return false;
		return a.Equals(b);
	}

	public static bool operator !=(Error? a, Error? b) => !(a == b);

	public bool Equals(Error? other)
	{
		if (other is null) return false;
		return other.Code == Code && other.Message == Message;
	}

	public override bool Equals(object? obj) => obj is Error error && Equals(error);

	public override int GetHashCode() => HashCode.Combine(Code, Message);

	public override string ToString() => Code;
}
