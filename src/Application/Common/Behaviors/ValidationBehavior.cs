using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
    {
        if(!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = (await Task.WhenAll(validators.Select(async validator => await validator.ValidateAsync(context, cancellationToken))))
                               .Where(validationResult => validationResult.Errors.Any())
                               .SelectMany(validationResult => validationResult.Errors)
                               .ToList();

        if(validationErrors.Count != 0)
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next();
    }
}