using System;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AppDomain.Enums;
using AppDomain.Services;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<Guid>, IMapTo<ToDoTask>
{
    public string Name { get; set; }

    public TaskState State { get; set; }

    public TaskPriority Priority { get; set; }

    public Guid? AssignedPersonId { get; set; }

    public void Mapping(Profile profile) =>
        profile.CreateMap<CreateTaskCommand, ToDoTask>()
               .ForMember(d => d.AssignedPersonId, o => o.Ignore());
}

public class CreateTaskCommandHandler(IRepository<ToDoTask, Guid> taskRepository, 
                                      IRepository<Person, Guid> personRepository, 
                                      ITaskManger taskManager, 
                                      IMapper mapper)
    : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = mapper.Map<ToDoTask>(request);

        if (request.AssignedPersonId != null)
        {
            var person = await personRepository.GetFirst(request.AssignedPersonId.Value);
            await taskManager.AssignTaskToPerson(task, person);
        }

        await taskRepository.Add(task);

        await taskRepository.Commit(cancellationToken);

        return task.Id;
    }
}