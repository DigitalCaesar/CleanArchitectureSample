using Data;
using Data.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Idempotent;
public sealed class IdempotentDomainEventHandler<TDomainEvent>
    : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> mDecorator;
    private readonly ApplicationDbContext mDbContext;

    public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorator, ApplicationDbContext dbContext)
    {
        mDecorator = decorator;
        mDbContext = dbContext;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        // Get the name of the consumer
        string consumer = mDecorator.GetType().Name;

        // Check if the event was already consumed
        bool EventConsumerMessage = await mDbContext.Set<EventConsumer>()
            .AnyAsync(
                eventConsumer => 
                    eventConsumer.Id == notification.Id &&
                    eventConsumer.Name == consumer, cancellationToken);

        // If it was, then no need to process
        if (EventConsumerMessage)
            return;

        // Handle the event
        await mDecorator.Handle(notification, cancellationToken);

        // Save the consumer event so we know it completed
        mDbContext.Set<EventConsumer>()
            .Add(new EventConsumer
            {
                Id = notification.Id,
                Name = consumer
            });
        await mDbContext.SaveChangesAsync(cancellationToken);
    }
}
