using System.Reflection;
using AppDomain.Common.Interfaces;
using Application.Common.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly, typeof(IRepository<,>).Assembly);

            configuration.AddOpenBehavior(typeof(RequestPerformanceBehaviour<,>));

            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));

            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
            
        services.AddValidatorsFromAssembly(typeof(Application.Common.Exceptions.ValidationException).Assembly);

        return services;
    }
}