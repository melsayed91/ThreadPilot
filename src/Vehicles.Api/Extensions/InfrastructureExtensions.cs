using Vehicles.Application.Ports;
using Vehicles.Infrastructure.Adapters;

namespace Vehicles.Api.Extensions;

internal static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IVehicleDataSource, VehicleDataSource>();
        return services;
    }
}
