using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Vehicles.Domain.ValueObjects;
using Vehicles.Infrastructure.Adapters;
using Vehicles.Infrastructure.Persistence;
using Vehicles.Infrastructure.Tests.TestSupport;

namespace Vehicles.Infrastructure.Tests.Adapters;

public class VehicleDataSourceTests
{
    [Fact]
    public async Task Can_lookup_single_and_batch_by_reg()
    {
        var (conn, seedCtx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            // Act
            await using var queryCtx = new VehiclesDbContext(
                new DbContextOptionsBuilder<VehiclesDbContext>().UseSqlite(conn).Options);

            var adapter = new VehicleDataSource(queryCtx);

            var one = await adapter.GetByRegAsync(RegistrationNumber.From("abc123"), CancellationToken.None);
            var batch = await adapter.GetByRegsAsync([
                RegistrationNumber.From("ABC123"),
                RegistrationNumber.From("xyz999"),
                RegistrationNumber.From("missing")
            ], CancellationToken.None);

            // Assert
            one.Should().NotBeNull();
            one.Make.Should().Be("Tesla");

            batch.Should().ContainKeys("ABC123", "XYZ999");
            batch.Should().NotContainKey("missing");
        }
        finally
        {
            await seedCtx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }

    [Fact]
    public async Task Returns_null_for_missing_and_empty_map_for_empty_input()
    {
        var (conn, ctx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            var adapter = new VehicleDataSource(ctx);

            var missing = await adapter.GetByRegAsync(RegistrationNumber.From("NOPE123"), CancellationToken.None);
            missing.Should().BeNull();

            var map = await adapter.GetByRegsAsync(Array.Empty<RegistrationNumber>(), CancellationToken.None);
            map.Should().BeEmpty();
        }
        finally
        {
            await ctx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }

    [Fact]
    public async Task Lookup_is_case_insensitive_due_to_VO_normalization()
    {
        var (conn, ctx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            var adapter = new VehicleDataSource(ctx);

            var hit = await adapter.GetByRegAsync(RegistrationNumber.From("abc123"), CancellationToken.None);
            hit.Should().NotBeNull();
            hit.RegNumber.Value.Should().Be("ABC123");
        }
        finally
        {
            await ctx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }
}
