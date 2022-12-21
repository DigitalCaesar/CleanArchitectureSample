using MediatR;

namespace Domain.Shared;
public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    //Task Handle(TDomainEvent notification, CancellationToken cancellationToken);
}
