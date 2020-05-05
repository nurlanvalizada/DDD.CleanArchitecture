using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Events;
using MediatR;

namespace AppDomain.Handlers
{
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
}
