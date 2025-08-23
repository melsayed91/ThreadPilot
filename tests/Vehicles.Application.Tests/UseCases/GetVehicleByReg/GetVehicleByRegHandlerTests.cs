using FluentAssertions;
using Moq;
using Vehicles.Application.Dtos;
using Vehicles.Application.Ports;
using Vehicles.Application.UseCases.GetVehicleByReg;
using Vehicles.Domain;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Application.Tests.UseCases.GetVehicleByReg;

public class GetVehicleByRegHandlerTests
{
    [Fact]
    public async Task Returns_vehicle_dto_when_found()
    {
        var src = new Mock<IVehicleDataSource>();
        src.Setup(s => s.GetByRegAsync(It.IsAny<RegistrationNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Vehicle(RegistrationNumber.From("ABC123"), "Tesla", "Model 3", 2022, "VIN1"));

        var handler = new GetVehicleByRegHandler(src.Object);

        var dto = await handler.Handle(new GetVehicleByRegQuery("ABC123"), CancellationToken.None);

        dto.Should().Be(new VehicleDto("ABC123", "Tesla", "Model 3", 2022, "VIN1"));
    }

    [Fact]
    public async Task Throws_not_found_when_missing()
    {
        var src = new Mock<IVehicleDataSource>();
        src.Setup(s => s.GetByRegAsync(It.IsAny<RegistrationNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle?)null);

        var handler = new GetVehicleByRegHandler(src.Object);

        var act = async () => await handler.Handle(new GetVehicleByRegQuery("MISSING"), CancellationToken.None);
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*not found*");
    }
}
