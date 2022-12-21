using Domain.ValueObjects;
using FluentValidation;

namespace Application.Members.Commands.Create;

internal class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().Must(x => !x.Any(x => char.IsWhiteSpace(x))).MaximumLength(UserName.MaxLength);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(Email.MaxLength);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(LastName.MaxLength);
    }
}
