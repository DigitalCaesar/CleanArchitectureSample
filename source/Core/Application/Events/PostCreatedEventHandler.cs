using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Events;
using Domain.Shared;
using MediatR;
using Messaging;

namespace Application.Events;
internal sealed class PostCreatedEventHandler : IDomainEventHandler<PostCreatedEvent>
{
    private readonly IEmailService mEmailService;
    private readonly IPostRepository mPostRepository;
    private readonly IMemberRepository mMemberRepository;

    public PostCreatedEventHandler(IEmailService emailService, IPostRepository postRepository, IMemberRepository memberRepository)
    {
        mEmailService = emailService;
        mPostRepository = postRepository;
        mMemberRepository = memberRepository;
    }

    public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        Post? Post = await mPostRepository.GetByIdAsync(notification.Id, cancellationToken);

        if (Post is null)
            return;  

        Member? Author = await mMemberRepository.GetByIdAsync(Post.AuthorId, cancellationToken);

        if (Author is null)
            return;  

        await mEmailService.SendEmailNotificationAsync($"A new post '{Post.Name}' was created by '{Author.Username}'.", cancellationToken);
    }
}
