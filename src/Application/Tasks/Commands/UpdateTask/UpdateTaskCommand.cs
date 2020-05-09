using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AppDomain.Enums;
using Application.Common.Exceptions;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest, IMapTo<ToDoTask>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TaskPriority Priority { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly IRepository<ToDoTask, int> _taskRepository;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(IRepository<ToDoTask, int> taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetFirst(request.Id);

            if (task == null)
            {
                throw new NotFoundException(nameof(ToDoTask), request.Id);
            }

            _mapper.Map(request, task);

            await _taskRepository.Commit(cancellationToken);

            return Unit.Value;
        }
    }
}
