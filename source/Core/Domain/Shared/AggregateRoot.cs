using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared;
public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> mDomainEvents = new();
    protected AggregateRoot(Guid id) : base(id) { }

    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => mDomainEvents.ToList();

    public void ClearDomainEvents() => mDomainEvents.Clear();

    protected void RaiseDomainEvent(DomainEvent domainEvent) =>
        mDomainEvents.Add(domainEvent);
}
