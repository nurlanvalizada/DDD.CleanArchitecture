namespace AppDomain.Common.Entities;

public abstract class Entity<T> : IEntity
{
    public T Id { get; init; }
}