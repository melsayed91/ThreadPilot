using FluentAssertions;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;
using Vehicles.Infrastructure.Adapters;

namespace Vehicles.Infrastructure.Tests.Adapters;

public class InMemoryVehicleDataSourceTests
{
    [Fact]
    public async Task Can_lookup_single_and_batch_by_reg()
    {
        var seed = new[]
        {
            new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), "Tesla", "3", 2020, "VIN-A"),
            new Vehicle(Guid.NewGuid(), RegistrationNumber.From("XYZ999"), "Volvo", "XC90", 2019, "VIN-X")
        };

        var src = new InMemoryVehicleDataSource(seed);

        var one = await src.GetByRegAsync(RegistrationNumber.From("abc123"), CancellationToken.None);
        one!.Make.Should().Be("Tesla");

        var batch = await src.GetByRegsAsync([
            RegistrationNumber.From("ABC123"),
            RegistrationNumber.From("xyz999"),
            RegistrationNumber.From("missing")
        ], CancellationToken.None);

        batch.Should().ContainKeys("ABC123", "XYZ999");
        batch.Should().NotContainKey("missing");
    }
}
