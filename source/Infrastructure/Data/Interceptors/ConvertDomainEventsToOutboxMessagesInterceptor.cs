using Data.Models;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Data.Interceptors;
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        // If no context then nothing to do
        if(dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        // Get the event, 
        var OutboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new DomainEventData
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
        dbContext.Set<DomainEventData>().AddRange(OutboxMessages);

        // Save changes
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
