using System;
using System.Collections.Generic;
using System.Linq;
using AppDomain.Common.DomainEvents;

namespace AppDomain.Common.Entities;

public abstract class AggregateRoot<T> : Entity<T>, IHaveDomainEvents
{
    private readonly ICollection<BaseDomainEvent> _events = [];
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
        
    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
        
    public IReadOnlyList<BaseDomainEvent> GetDomainEvents()
    {
        return _events.ToList();
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}