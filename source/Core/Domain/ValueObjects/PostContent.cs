using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects;
public sealed class PostContent : ValueObject
{
    public const int MaxLength = 50000;

    public string Value { get; }

    private PostContent(string value)
    {
        Value = value;
    }

    public static Result<PostContent> Create(string value)
    {
        const string ShortName = nameof(PostContent);
        const string LongName = "post content";

        if (string.IsNullOrEmpty(value))
            return Result.Failure<PostContent>(new Error($"{ShortName}.Empty", $"The {LongName} must have a value."));
        if (value.Length > MaxLength)
            return Result.Failure<PostContent>(new Error($"{ShortName}.TooLong", $"The {LongName} must be less that {MaxLength} characters in length."));
        return new PostContent(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
