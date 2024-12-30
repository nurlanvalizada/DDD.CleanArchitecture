using System.Collections.Generic;

namespace AppDomain.Common.DomainEvents;

public interface IHaveDomainEvents
{
    void AddDomainEvent(BaseDomainEvent domainEvent);
    
    IReadOnlyList<BaseDomainEvent> GetDomainEvents();

    void ClearDomainEvents();
}