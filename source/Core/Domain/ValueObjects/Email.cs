using Domain.Errors;
using Domain.Shared;

namespace Domain.ValueObjects;
public sealed class Email : ValueObject
{
    public const int MaxLength = 254;

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        const string ShortName = nameof(Email);
        const string LongName = "email";

        return Result.Create(value)
            .Ensure(
                e => !string.IsNullOrWhiteSpace(e),
                DomainErrors.Email.Empty(ShortName, LongName))
            .Ensure(
                e => e.Length <= MaxLength,
                DomainErrors.Email.TooLong(ShortName, LongName, MaxLength))
            .Ensure(
                e => e.Split('@').Length == 2,
                DomainErrors.Email.InvalidFormat(ShortName, LongName))
            .Map(e => new Email(e));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
