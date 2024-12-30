using AppDomain.Common.DomainEvents;
using AppDomain.Entities;

namespace AppDomain.Events;

public class TaskCreatedEvent(ToDoTask completedTask) : BaseDomainEvent
{
    public ToDoTask CompletedTask { get; set; } = completedTask;
}