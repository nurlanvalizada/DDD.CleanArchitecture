using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Queries.GetTasks
{
    public class GetTasksQuery : IRequest<TasksVm>
    {
        public string Name { get; set; }
    }

    public class GetTasksQueryHandler(IMapper mapper, IRepository<ToDoTask, int> taskRepository) : IRequestHandler<GetTasksQuery, TasksVm>
    {
        public async Task<TasksVm> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var vm = new TasksVm
            {
                Tasks = await taskRepository.GetAll().Where(t=>t.Name.Contains(request.Name))
                    .ProjectTo<TaskDto>(mapper.ConfigurationProvider)
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken)
            };

            return vm;
        }
    }
}
