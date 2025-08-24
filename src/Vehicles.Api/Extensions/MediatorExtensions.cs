using System.Reflection;
using FluentValidation;
using MediatR;
using Vehicles.Application.Behaviors;

namespace Vehicles.Api.Extensions;

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
