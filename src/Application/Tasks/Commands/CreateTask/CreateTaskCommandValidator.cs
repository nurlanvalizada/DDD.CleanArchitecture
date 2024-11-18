using System;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using FluentValidation;

namespace Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private readonly IRepository<ToDoTask, Guid> _taskRepository;

    public CreateTaskCommandValidator(IRepository<ToDoTask, Guid> taskRepository)
    {
        _taskRepository = taskRepository;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified name already exists.");
    }

    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
    {
        return await _taskRepository.All(l => l.Name != name);
    }
}