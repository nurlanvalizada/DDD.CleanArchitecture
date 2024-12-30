using AppDomain.Common.DomainEvents;

namespace AppDomain.ToDoTasks.Events;

public class TaskCompletedEvent(ToDoTask completedTask) : BaseDomainEvent
{
    public ToDoTask CompletedTask { get; set; } = completedTask;
}