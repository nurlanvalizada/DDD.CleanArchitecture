using AppDomain.Common.DomainEvents;

namespace AppDomain.ToDoTasks.Events;

public class TaskCreatedEvent(ToDoTask completedTask) : BaseDomainEvent
{
    public ToDoTask CompletedTask { get; set; } = completedTask;
}