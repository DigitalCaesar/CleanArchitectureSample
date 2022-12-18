using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class UserName : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private UserName(string value)
    {
        Value = value;
    }

    public static Result<UserName> Create(string value)
    {
        const string ShortName = nameof(UserName);
        const string LongName = "user name";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<UserName>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<UserName>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new UserName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
