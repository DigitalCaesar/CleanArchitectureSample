
namespace Domain.Shared;
public abstract record DomainEvent(Guid Id) : IDomainEvent;
