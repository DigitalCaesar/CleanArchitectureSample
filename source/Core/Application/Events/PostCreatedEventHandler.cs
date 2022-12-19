using Domain.Entities.Posts;
using Domain.Events;
using MediatR;
using Messaging;

namespace Application.Events;
internal sealed class PostCreatedEventHandler : INotificationHandler<PostCreatedEvent>
{
    private readonly IEmailService mEmailService;
    private readonly IPostRepository mPostRepository;

    public PostCreatedEventHandler(IEmailService emailService, IPostRepository postRepository)
    {
        mEmailService = emailService;
        mPostRepository = postRepository;
    }

    public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        Post? Post = await mPostRepository.GetByIdAsync(notification.PostId, cancellationToken);

        if (Post is null)
            return;

        await mEmailService.SendEmailNotificationAsync($"A new post '{Post.Name}' was created by '{Post.Author.Username}'.", cancellationToken);
    }
}
