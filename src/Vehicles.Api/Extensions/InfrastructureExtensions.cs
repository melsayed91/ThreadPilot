using Vehicles.Application.Ports;
using Vehicles.Infrastructure.Adapters;

namespace Vehicles.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IVehicleDataSource, InMemoryVehicleDataSource>();
        return services;
    }
}
