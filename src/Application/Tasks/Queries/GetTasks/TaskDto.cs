using AppDomain.Entities;
using AppDomain.Enums;
using Application.Common.Mappings;

namespace Application.Tasks.Queries.GetTasks
{
    public class TaskDto : IMapFrom<ToDoTask>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskState State { get; set; }
    }
}
