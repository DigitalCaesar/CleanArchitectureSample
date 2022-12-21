using Data.Models;
using Domain;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data;
internal sealed class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext context;

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        UpdateAuditableEntities();

        return context.SaveChangesAsync();
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        // Get the event, 
        var OutboxMessages = context.ChangeTracker
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
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();

        // Add Outbox messages
        context.Set<DomainEventData>().AddRange(OutboxMessages);
    }
    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            context
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach(EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if(entityEntry.State == EntityState.Added)
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            if(entityEntry.State == EntityState.Modified)
                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
        }
    }
}
