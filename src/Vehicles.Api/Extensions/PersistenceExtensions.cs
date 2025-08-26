using Microsoft.EntityFrameworkCore;
using Vehicles.Infrastructure.Persistence;

namespace Vehicles.Api.Extensions;

internal static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        services.AddDbContext<VehiclesDbContext>(opts => { opts.UseNpgsql(cs); });

        services.AddHealthChecks()
            .AddDbContextCheck<VehiclesDbContext>("vehicles-db");

        return services;
    }

    public static IApplicationBuilder MigrateDatabaseOnStartup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var cfg = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!bool.TryParse(cfg["MIGRATE_ON_STARTUP"], out var migrate) || !migrate)
            return app;

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VehiclesDbContext>();

        db.Database.Migrate();
        return app;
    }
}
