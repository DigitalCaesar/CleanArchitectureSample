using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class PostName : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private PostName(string value)
    {
        Value = value;
    }

    public static Result<PostName> Create(string value)
    {
        const string ShortName = nameof(PostName);
        const string LongName = "post name";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<PostName>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<PostName>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new PostName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
