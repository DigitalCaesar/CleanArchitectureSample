using Domain.Entities.Members;
using Domain.Shared;

namespace Domain.Entities.Events;
public interface IEventRepository
{
    Task AddEventAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken);
    Task<List<IDomainEventData>> GetAll(CancellationToken cancellationToken);
}
