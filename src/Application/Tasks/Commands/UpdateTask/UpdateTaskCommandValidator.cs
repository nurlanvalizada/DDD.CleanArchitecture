using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppDomain.Common.Interfaces;
using AppDomain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    private readonly IRepository<ToDoTask, Guid> _todoListRepository;

    public UpdateTaskCommandValidator(IRepository<ToDoTask, Guid> todoListRepository)
    {
        _todoListRepository = todoListRepository;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 50 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified name is already exists.");
    }

    private async Task<bool> BeUniqueTitle(UpdateTaskCommand model, string name, CancellationToken cancellationToken)
    {
        return await _todoListRepository.AllAsync(l => l.Id != model.Id && l.Name != name, cancellationToken);
    }
}