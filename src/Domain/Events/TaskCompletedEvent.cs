using AppDomain.Common.DomainEvents;
using AppDomain.Entities;

namespace AppDomain.Events
{
    public class TaskCompletedEvent : BaseDomainEvent
    {
        public ToDoTask CompletedTask { get; set; }

        public TaskCompletedEvent(ToDoTask completedTask)
        {
            CompletedTask = completedTask;
        }
    }
}
