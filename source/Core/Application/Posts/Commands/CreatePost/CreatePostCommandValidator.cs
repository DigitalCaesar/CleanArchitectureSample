using Domain.ValueObjects;
using FluentValidation;

namespace Application.Posts.Commands.CreatePost;
internal class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(Name.MaxLength);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(Description.MaxLength);
        RuleFor(x => x.AuthorId).NotNull();
        RuleFor(x => x.Tags).NotNull();
    }
}
