using FluentAssertions;
using Moq;
using Vehicles.Application.Dtos;
using Vehicles.Application.Ports;
using Vehicles.Application.UseCases.GetVehiclesBatch;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Application.Tests.UseCases.GetVehiclesBatch;

public class GetVehiclesBatchHandlerTests
{
    [Fact]
    public async Task Returns_all_found_vehicles_in_batch_and_dedups_inputs()
    {
        var veh1 = new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), "Tesla", "3", 2020, "VIN-A");
        var veh2 = new Vehicle(Guid.NewGuid(), RegistrationNumber.From("XYZ999"), "Volvo", "XC90", 2019, "VIN-X");

        var src = new Mock<IVehicleDataSource>();
        src.Setup(s => s.GetByRegsAsync(It.IsAny<IEnumerable<RegistrationNumber>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, Vehicle>(StringComparer.OrdinalIgnoreCase)
            {
                [veh1.RegNumber.Value] = veh1,
                [veh2.RegNumber.Value] = veh2
            });

        var handler = new GetVehiclesBatchHandler(src.Object);

        var result = await handler.Handle(
            new GetVehiclesBatchQuery(["abc123", "ABC123", "xyz999"]),
            CancellationToken.None);

        result.Should().BeEquivalentTo([
            new VehicleDto("ABC123", "Tesla", "3", 2020, "VIN-A"),
            new VehicleDto("XYZ999", "Volvo", "XC90", 2019, "VIN-X")
        ], opts => opts.WithStrictOrdering());
    }
}
