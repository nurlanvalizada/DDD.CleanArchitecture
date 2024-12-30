using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AutoMapper;
using MediatR;

namespace Application.Tasks.Queries.GetTasks;

public record GetTasksQuery : IRequest<TasksVm>
{
    public string Name { get; set; }
}

public class GetTasksQueryHandler(IMapper mapper, IRepository<ToDoTask, Guid> taskRepository) : IRequestHandler<GetTasksQuery, TasksVm>
{
    public async Task<TasksVm> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await taskRepository.FilterAsync(t => string.IsNullOrWhiteSpace(request.Name) || t.Name.Contains(request.Name), cancellationToken);

        var tasksDto = mapper.Map<List<TaskDto>>(tasks.OrderBy(t => t.Name));
        
        var vm = new TasksVm
        {
            Tasks = tasksDto  
        };

        return vm;
    }
}