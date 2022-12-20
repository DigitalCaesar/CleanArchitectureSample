using Domain.Shared;

namespace Domain.Entities.Events;
public interface IEventRepository
{
    Task AddEventAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken);
}
