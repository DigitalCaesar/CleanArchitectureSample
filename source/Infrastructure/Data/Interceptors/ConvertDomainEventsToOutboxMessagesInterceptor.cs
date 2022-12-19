using Data.Models;
using Domain.Shared;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Data.Interceptors;
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        // If no context then nothing to do
        if(dbContext is null)
            return base.SavedChangesAsync(eventData, result, cancellationToken);

        // Get the event, 
        var OutboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregate =>
            {
                var domainEvents = aggregate.GetDomainEvents();

                aggregate.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc= DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent, 
                    new JsonSerializerSettings
                    {
                        TypeNameHandling= TypeNameHandling.All,
                    })
            })
            .ToList();

        // Add Outbox messages
        dbContext.Set<OutboxMessage>().AddRange(OutboxMessages);

        // Save changes
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
