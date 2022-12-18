using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class Description : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        const string ShortName = nameof(Description);
        const string LongName = "description";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<Description>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<Description>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new Description(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
