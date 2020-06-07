using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using AppDomain.Enums;
using AppDomain.Services;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<int>, IMapTo<ToDoTask>
    {
        public string Name { get; set; }

        public TaskState State { get; set; }

        public TaskPriority Priority { get; set; }

        public int? AssignedPersonId { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<CreateTaskCommand, ToDoTask>()
                .ForMember(d => d.AssignedPersonId, o => o.Ignore());
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IRepository<ToDoTask, int> _taskRepository;
        private readonly IRepository<Person, int> _personRepository;
        private readonly ITaskManger _taskManager;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(IRepository<ToDoTask, int> taskRepository, IRepository<Person, int> personRepository, ITaskManger taskManager, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _personRepository = personRepository;
            _taskManager = taskManager;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = _mapper.Map<ToDoTask>(request);

            if (request.AssignedPersonId != null)
            {
                var person = await _personRepository.GetFirst(request.AssignedPersonId.Value);
                await _taskManager.AssignTaskToPerson(task, person);
            }

            await _taskRepository.Add(task);

            await _taskRepository.Commit(cancellationToken);

            return task.Id;
        }
    }
}
