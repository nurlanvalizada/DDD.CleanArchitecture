using System;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AppDomain.Enums;
using Application.Common.Exceptions;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest, IMapTo<ToDoTask>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TaskPriority Priority { get; set; }

    public bool IsComplete { get; set; }
}

public class UpdateTaskCommandHandler(IRepository<ToDoTask, Guid> taskRepository, IMapper mapper) : IRequestHandler<UpdateTaskCommand>
{
    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetFirstAsync(request.Id, cancellationToken);

        if (task == null)
        {
            throw new NotFoundException(nameof(ToDoTask), request.Id);
        }

        mapper.Map(request, task);

        if (request.IsComplete)
            task.MarkComplete();
        else
            task.MarkUnComplete();

        await taskRepository.CommitAsync(cancellationToken);
    }
}