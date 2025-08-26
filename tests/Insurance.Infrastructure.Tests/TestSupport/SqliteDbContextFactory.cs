using Insurance.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Tests.TestSupport;

internal static class SqliteDbContextFactory
{
    public static (SqliteConnection conn, InsuranceDbContext ctx) CreateOpen()
    {
        var conn = new SqliteConnection("Data Source=:memory:");
        conn.Open();

        var opts = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseSqlite(conn)
            .Options;

        var ctx = new InsuranceDbContext(opts);
        ctx.Database.EnsureCreated();

        return (conn, ctx);
    }
}
