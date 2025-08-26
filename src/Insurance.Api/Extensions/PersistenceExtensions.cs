using Insurance.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Api.Extensions;

internal static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("Missing connection string 'Default'.");
        services.AddDbContext<InsuranceDbContext>(opts => opts.UseNpgsql(cs));
        services.AddHealthChecks().AddDbContextCheck<InsuranceDbContext>("insurance-db");
        return services;
    }

    public static IApplicationBuilder MigrateDatabaseOnStartup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var cfg = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (!bool.TryParse(cfg["MIGRATE_ON_STARTUP"], out var migrate) || !migrate)
            return app;

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

        db.Database.Migrate();
        return app;
    }
}
