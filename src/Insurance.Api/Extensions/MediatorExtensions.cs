using System.Reflection;
using FluentValidation;
using Insurance.Application.Behaviors;
using MediatR;

namespace Insurance.Api.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
