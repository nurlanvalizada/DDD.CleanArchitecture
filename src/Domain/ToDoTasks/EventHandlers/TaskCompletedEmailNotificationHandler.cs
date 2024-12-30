using System;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.ToDoTasks.Events;
using MediatR;

namespace AppDomain.ToDoTasks.EventHandlers;

public class TaskCompletedEmailNotificationHandler : INotificationHandler<TaskCompletedEvent>
{
    public Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        // Do Nothing
        return Task.CompletedTask;
    }
}