using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler(IRepository<ToDoTask, int> taskRepository) : IRequestHandler<DeleteTaskCommand>
    {
        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await taskRepository.GetFirst(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ToDoTask), request.Id);
            }

            await taskRepository.Delete(entity);

            await taskRepository.Commit(cancellationToken);
        }
    }
}
