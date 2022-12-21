using Domain.Shared;

namespace Domain.Events;
public sealed record MemberCreatedEvent(Guid Id) : DomainEvent(Id);
