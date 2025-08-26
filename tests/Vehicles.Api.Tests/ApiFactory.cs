using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vehicles.Application.Ports;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;
using Vehicles.Infrastructure.Adapters;
using Vehicles.Infrastructure.Persistence;

namespace Vehicles.Api.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<VehiclesDbContext>();
            services.RemoveAll<IVehicleDataSource>();

            var seed = new[]
            {
                new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), "Tesla", "Model 3", 2022, "VIN1"),
                new Vehicle(Guid.NewGuid(), RegistrationNumber.From("XYZ999"), "Volvo", "XC90", 2019, "VIN-X")
            };

            services.AddSingleton<IVehicleDataSource>(new InMemoryVehicleDataSource(seed));
        });
    }
}
