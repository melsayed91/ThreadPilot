using FluentAssertions;
using Insurance.Domain.ValueObjects;
using Insurance.Infrastructure.Adapters;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Tests.TestSupport;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Tests.Adapters;

public class InsuranceDataAdapterTests
{
    [Fact]
    public async Task Gets_policies_for_person_and_orders_by_type()
    {
        var (conn, seedCtx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            await using var queryCtx = new InsuranceDbContext(
                new DbContextOptionsBuilder<InsuranceDbContext>().UseSqlite(conn).Options);

            var adapter = new InsuranceDataAdapter(queryCtx);

            // Act
            var list = await adapter.GetPoliciesAsync(" 19650101-1234 ", CancellationToken.None);

            list.Should().HaveCount(4);

            list.Select(p => p.Type)
                .Should().Equal(PolicyType.Pet, PolicyType.PersonalHealth, PolicyType.Car, PolicyType.Car);

            list.Sum(p => p.MonthlyCost.Amount).Should().Be(10 + 20 + 30 + 30);

            var carRegs = list.Where(p => p.Type == PolicyType.Car)
                .Select(p => p.VehicleRegNumber!)
                .Distinct()
                .OrderBy(s => s)
                .ToArray();

            carRegs.Should().Equal("ABC123", "XYZ999");

            list.Should().OnlyContain(p => p.PersonalNumber.Value == "19650101-1234" && p.Id != Guid.Empty);
        }
        finally
        {
            await seedCtx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }

    [Fact]
    public async Task Returns_empty_for_missing_personal_number()
    {
        var (conn, ctx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            var adapter = new InsuranceDataAdapter(ctx);
            var list = await adapter.GetPoliciesAsync("19990101-9999", CancellationToken.None);
            list.Should().BeEmpty();
        }
        finally
        {
            await ctx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }

    [Fact]
    public async Task Throws_for_blank_personal_number()
    {
        var (conn, ctx) = SqliteDbContextFactory.CreateOpen();
        try
        {
            var adapter = new InsuranceDataAdapter(ctx);
            var act = async () => await adapter.GetPoliciesAsync("  ", CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithParameterName("personalNumber");
        }
        finally
        {
            await ctx.DisposeAsync();
            await conn.DisposeAsync();
        }
    }
}
