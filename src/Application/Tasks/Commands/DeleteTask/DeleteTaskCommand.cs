using System;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTaskCommandHandler(IRepository<ToDoTask, Guid> taskRepository) : IRequestHandler<DeleteTaskCommand>
{
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await taskRepository.GetFirstAsync(request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ToDoTask), request.Id);
        }

        await taskRepository.DeleteAsync(entity);

        await taskRepository.CommitAsync(cancellationToken);
    }
}