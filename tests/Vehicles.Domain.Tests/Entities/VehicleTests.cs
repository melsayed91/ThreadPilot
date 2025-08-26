using FluentAssertions;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Domain.Tests.Entities;

public class VehicleTests
{
    [Fact]
    public void Creates_vehicle_with_normalized_reg_and_trimmed_fields()
    {
        var reg = RegistrationNumber.From("  abC123 ");
        var v = new Vehicle(Guid.NewGuid(), reg, "  Tesla  ", "  Model 3 ", 2022, " 5YJ3E1EA7KF317000 ");

        v.RegNumber.Value.Should().Be("ABC123");
        v.Make.Should().Be("Tesla");
        v.Model.Should().Be("Model 3");
        v.Year.Should().Be(2022);
        v.Vin.Should().Be("5YJ3E1EA7KF317000");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Empty_make_throws(string? make)
    {
        var act = () => new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), make!, "Model", 2020, "VIN");
        act.Should().Throw<DomainException>().WithMessage("*Make is required*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Empty_model_throws(string? model)
    {
        var act = () => new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), "Tesla", model!, 2020, "VIN");
        act.Should().Throw<DomainException>().WithMessage("*Model is required*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Empty_vin_throws(string? vin)
    {
        var act = () => new Vehicle(Guid.NewGuid(), RegistrationNumber.From("ABC123"), "Tesla", "Model 3", 2020, vin!);
        act.Should().Throw<DomainException>().WithMessage("*VIN is required*");
    }
}
