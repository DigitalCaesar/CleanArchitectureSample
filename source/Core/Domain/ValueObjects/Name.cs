using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class Name : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        const string ShortName = nameof(FirstName);
        const string LongName = "name";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<Name>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<Name>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new Name(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
