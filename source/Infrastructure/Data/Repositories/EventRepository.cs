using Data.Mapping;
using Data.Models;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data.Repositories;
public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext mDataContext;

    public EventRepository(ApplicationDbContext dataContext)
    {
        mDataContext = dataContext;
    }

    public async Task AddEventAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        var events = aggregateRoot.GetDomainEvents();
        aggregateRoot.ClearDomainEvents();
        var OutboxMessages = events.Select(domainEvent => new DomainEventData
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
        mDataContext.Set<DomainEventData>().AddRange(OutboxMessages);
    }

    public async Task<List<IDomainEventData>> GetAll(CancellationToken cancellationToken)
    {
        List<DomainEventData> RawData = await mDataContext.DomainEvents.ToListAsync();
        List<IDomainEventData> MappedData = RawData.ToList<IDomainEventData>();//.Select(x => (IDomainEventData)x);
        return MappedData;
    }
}
