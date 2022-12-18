using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private LastName(string value)
    {
        Value = value;
    }

    public static Result<LastName> Create(string value)
    {
        const string ShortName = nameof(LastName);
        const string LongName = "last name";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<LastName>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<LastName>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new LastName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
