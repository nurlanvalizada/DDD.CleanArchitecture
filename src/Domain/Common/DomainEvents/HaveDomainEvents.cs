using System.Collections.Generic;

namespace AppDomain.Common.DomainEvents
{
    public abstract class HaveDomainEvents
    {
        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}