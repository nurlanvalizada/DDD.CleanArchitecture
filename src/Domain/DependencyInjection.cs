using AppDomain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AppDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ITaskManger, TaskManager>();

        return services;
    }
}