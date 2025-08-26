using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Vehicles.Infrastructure.Persistence;

public sealed class VehiclesDbContextFactory : IDesignTimeDbContextFactory<VehiclesDbContext>
{
    public VehiclesDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<VehiclesDbContext>();

        // Prefer env var in CI
        var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                 ?? "Host=localhost;Port=5432;Database=__design_time__;Username=dev;Password=dev";

        builder.UseNpgsql(cs);
        return new VehiclesDbContext(builder.Options);
    }
}
