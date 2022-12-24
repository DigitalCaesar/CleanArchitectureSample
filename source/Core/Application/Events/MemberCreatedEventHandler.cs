using Domain.Entities.Members;
using Domain.Events;
using MediatR;
using Infrastructure.Messaging;

namespace Application.Events;
internal sealed class MemberCreatedEventHandler : INotificationHandler<MemberCreatedEvent>
{
    private readonly IEmailService mEmailService;
    private readonly IMemberRepository mMemberRepository;

    public MemberCreatedEventHandler(IEmailService emailService, IMemberRepository postRepository)
    {
        mEmailService = emailService;
        mMemberRepository = postRepository;
    }

    public async Task Handle(MemberCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        Member? NewMember = await mMemberRepository.GetByIdAsync(notification.Id, cancellationToken);

        if (NewMember is null)
            return;

        await mEmailService.SendEmailNotificationAsync($"A new member '{NewMember.Username}' was added with email address '{NewMember.Email}'.", cancellationToken);
    }
}
