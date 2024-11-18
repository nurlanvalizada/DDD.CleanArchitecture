using System.Collections.Generic;
using System.Linq;

namespace AppDomain.Common.DomainEvents;

public abstract class HaveDomainEvents
{
    private readonly List<BaseDomainEvent> _events = [];
        
    protected void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
        
    public IReadOnlyList<BaseDomainEvent> GetDomainEvents()
    {
        return _events.ToList().AsReadOnly();
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}