using System.Threading.Tasks;
using AppDomain.Entities;

namespace AppDomain.ToDoTasks.Services;

public interface ITaskManger
{
    Task AssignTaskToPerson(ToDoTask task, Person person);
}