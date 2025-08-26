using Microsoft.EntityFrameworkCore;
using Vehicles.Infrastructure.Persistence.Configurations;
using Vehicles.Infrastructure.Persistence.Entities;

namespace Vehicles.Infrastructure.Persistence;

public class VehiclesDbContext : DbContext
{
    public VehiclesDbContext(DbContextOptions<VehiclesDbContext> options) : base(options)
    {
    }


    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfiguration(new VehicleConfig());
    }
}
