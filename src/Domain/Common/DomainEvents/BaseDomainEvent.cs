using System;
using MediatR;

namespace AppDomain.Common.DomainEvents
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
