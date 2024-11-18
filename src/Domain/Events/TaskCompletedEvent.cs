using AppDomain.Common.DomainEvents;
using AppDomain.Entities;

namespace AppDomain.Events;

public class TaskCompletedEvent(ToDoTask completedTask) : BaseDomainEvent
{
    public ToDoTask CompletedTask { get; set; } = completedTask;
}