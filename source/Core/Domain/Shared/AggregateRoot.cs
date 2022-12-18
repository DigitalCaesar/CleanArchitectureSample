using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared;
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> mDomainEvents = new();
    protected AggregateRoot(Guid id) : base(id) { }

    protected void RaiseDomainEvent(IDomainEvent @event)
    {
        mDomainEvents.Add(@event);
    }
}
