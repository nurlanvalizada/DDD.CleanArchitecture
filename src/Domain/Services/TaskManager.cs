using System;
using System.Threading.Tasks;
using AppDomain.Common.Exceptions;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AppDomain.Enums;

namespace AppDomain.Services;

public class TaskManager(IRepository<ToDoTask, Guid> taskRepository) : ITaskManger
{
    public const int MaxActiveTaskCountForAPerson = 3;

    public async Task AssignTaskToPerson(ToDoTask task, Person person)
    {
        if (task.AssignedPersonId == person.Id)
        {
            return;
        }

        if (task.State != TaskState.Active)
        {
            throw new ApplicationException("Can not assign a task to a person when task is not active!");
        }

        if (await HasPersonMaximumAssignedTask(person))
        {
            throw new UserFriendlyException($"{person.Name} already have at most {MaxActiveTaskCountForAPerson} active tasks");
        }

        task.AssignedPersonId = person.Id;
    }

    private async Task<bool> HasPersonMaximumAssignedTask(Person person)
    {
        var assignedTaskCount = await taskRepository.Count(t => t.State == TaskState.Active && t.AssignedPersonId == person.Id);
        return assignedTaskCount >= MaxActiveTaskCountForAPerson;
    }
}