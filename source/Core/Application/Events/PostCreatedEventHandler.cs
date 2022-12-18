using Domain.Entities.Posts;
using Domain.Events;
using MediatR;
using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Events;
internal sealed class PostCreatedEventHandler : INotificationHandler<PostCreatedEvent>
{
    private readonly IEmailService mEmailService;
    private readonly IPostRepository mPostRepository;

    public async Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
    {
        Post Post = await mPostRepository.GetByIdAsync(notification.PostId, cancellationToken);

        if (Post is null)
            return;

        await mEmailService.SendEmailNotificationAsync($"A new post '{Post.Name}' was created by '{Post.Author.Username}'.", cancellationToken);
    }
}
