using Domain.Shared;

namespace Domain.Events;
public sealed record PostCreatedEvent(Guid Id, Guid IMemberId) : DomainEvent(Id);
