using Insurance.Application.Ports;
using Insurance.Infrastructure.Adapters;
using Insurance.Infrastructure.Decorators;

namespace Insurance.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IInsuranceDataPort, InMemoryInsuranceDataAdapter>();
        services.AddHttpClient<IVehicleLookupPort, VehicleLookupHttpAdapter>(client =>
        {
            var baseUrl = config["Vehicles:BaseUrl"] ?? "http://localhost:5011";
            client.BaseAddress = new Uri(baseUrl);
        });
        services.Decorate<IVehicleLookupPort, FeatureGatedVehicleLookup>();


        return services;
    }
}
