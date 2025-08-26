using Insurance.Infrastructure.Persistence.Configurations;
using Insurance.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Persistence;

public sealed class InsuranceDbContext : DbContext
{
    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options)
    {
    }

    public DbSet<InsurancePolicyEntity> Policies => Set<InsurancePolicyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfiguration(new InsurancePolicyEntityConfig());
    }
}
