using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Vehicles.Infrastructure.Persistence;

namespace Vehicles.Infrastructure.Tests.TestSupport;

internal static class SqliteDbContextFactory
{
    public static (SqliteConnection conn, VehiclesDbContext ctx) CreateOpen()
    {
        var conn = new SqliteConnection("Data Source=:memory:");
        conn.Open();

        var opts = new DbContextOptionsBuilder<VehiclesDbContext>()
            .UseSqlite(conn)
            .Options;

        var ctx = new VehiclesDbContext(opts);
        ctx.Database.EnsureCreated();
        return (conn, ctx);
    }
}
