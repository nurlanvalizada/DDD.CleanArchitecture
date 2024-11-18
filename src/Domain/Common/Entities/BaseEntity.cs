using AppDomain.Common.DomainEvents;

namespace AppDomain.Common.Entities;

public abstract class BaseEntity<T> : HaveDomainEvents, IEntity
{
    public T Id { get; init; }
}