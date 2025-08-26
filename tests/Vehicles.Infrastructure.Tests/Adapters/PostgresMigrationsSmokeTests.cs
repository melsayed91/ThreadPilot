using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Vehicles.Infrastructure.Persistence;
using Vehicles.Infrastructure.Persistence.Entities;

namespace Vehicles.Infrastructure.Tests.Adapters;

public class PostgresMigrationsSmokeTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _pg = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("vehicles")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public Task InitializeAsync() => _pg.StartAsync();
    public Task DisposeAsync() => _pg.DisposeAsync().AsTask();

    [Fact]
    public async Task Migrations_apply_and_unique_indexes_enforced()
    {
        var opts = new DbContextOptionsBuilder<VehiclesDbContext>()
            .UseNpgsql(_pg.GetConnectionString())
            .Options;

        var ct = CancellationToken.None;

        await using (var db = new VehiclesDbContext(opts))
        {
            await db.Database.MigrateAsync(ct);

            db.Vehicles.Add(new VehicleEntity
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "ABC123",
                Make = "Volvo",
                Model = "XC90",
                Year = 2019,
                Vin = "VIN-NEW-1"
            });

            var dupRegThrew = false;
            try
            {
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                dupRegThrew = true;
            }

            dupRegThrew.Should().BeTrue("duplicate RegistrationNumber should violate the unique index");
        }

        await using (var db = new VehiclesDbContext(opts))
        {
            db.Vehicles.Add(new VehicleEntity
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "ZZZ777",
                Make = "BMW",
                Model = "X5",
                Year = 2021,
                Vin = "VIN-X"
            });

            var dupVinThrew = false;
            try
            {
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                dupVinThrew = true;
            }

            dupVinThrew.Should().BeTrue("duplicate VIN should violate the unique index");
        }
    }
}
