using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Insurance.Infrastructure.Persistence;

public sealed class InsuranceDbContextFactory : IDesignTimeDbContextFactory<InsuranceDbContext>
{
    public InsuranceDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                 ?? "Host=localhost;Port=5432;Database=insurance_db;Username=appuser;Password=appsecret";
        var b = new DbContextOptionsBuilder<InsuranceDbContext>().UseNpgsql(cs);
        return new InsuranceDbContext(b.Options);
    }
}
