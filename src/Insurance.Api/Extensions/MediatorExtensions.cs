using System.Reflection;
using FluentValidation;
using MediatR;
using Shared.Behaviors;

namespace Insurance.Api.Extensions;

internal static class MediatorExtensions
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
