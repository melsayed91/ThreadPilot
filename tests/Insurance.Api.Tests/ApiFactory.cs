using Insurance.Application.Ports;
using Insurance.Infrastructure.Adapters;
using Insurance.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insurance.Api.Tests;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<InsuranceDbContext>();
            services.RemoveAll<IInsuranceDataPort>();

            services.AddSingleton<IInsuranceDataPort, InMemoryInsuranceDataAdapter>();
        });
    }
}
