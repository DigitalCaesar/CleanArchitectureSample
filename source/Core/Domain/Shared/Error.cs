using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared;
public class Error : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public override bool Equals(object? obj)
    {
        // Check if null
        if (obj is null)
        {
            return false;
        }

        // Check for incompatible types
        if (obj.GetType() != GetType())
        {
            return false;
        }

        // Check if not same type 
        if (obj is not Error error)
        {
            return false;
        }

        // Finally, compare the two objects
        return error.Code == Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode() * 41;
    }

    public bool Equals(Error? other)
    {
        // Check if null
        if (other is null)
        {
            return false;
        }

        // Check for incompatible types
        if (other.GetType() != GetType())
        {
            return false;
        }

        // Finally, compare the two objects
        return other.Code == Code;
    }
    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? left, Error? right)
    {
        return (left is not null && right is not null && left.Equals(right));
    }
    public static bool operator !=(Error? left, Error? right)
    {
        return !(left == right);
    }
}
