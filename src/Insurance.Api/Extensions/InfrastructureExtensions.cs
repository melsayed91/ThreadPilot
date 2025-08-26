using Insurance.Application.Ports;
using Insurance.Infrastructure.Adapters;
using Insurance.Infrastructure.Concurrency;
using Insurance.Infrastructure.Decorators;

namespace Insurance.Api.Extensions;

internal static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<SingleFlightCoordinator>();
        services.AddScoped<IInsuranceDataPort, InsuranceDataAdapter>();

        services.AddHttpClient<IVehicleLookupPort, VehicleLookupHttpAdapter>(client =>
        {
            var baseUrl = config["Vehicles:BaseUrl"] ?? "http://localhost:5011";
            client.BaseAddress = new Uri(baseUrl);
        });

        services.Decorate<IVehicleLookupPort, CoalescingVehicleLookupAdapter>();
        services.Decorate<IVehicleLookupPort, FeatureGatedVehicleLookup>();

        return services;
    }
}
