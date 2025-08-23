using FluentAssertions;
using Vehicles.Application.UseCases.GetVehicleByReg;

namespace Vehicles.Application.Tests.UseCases.GetVehicleByReg;

public class GetVehicleByRegValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Rejects_empty_reg(string reg)
    {
        var v = new GetVehicleByRegValidator();
        var result = v.Validate(new GetVehicleByRegQuery(reg));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Rejects_too_long_reg()
    {
        var v = new GetVehicleByRegValidator();
        var result = v.Validate(new GetVehicleByRegQuery(new string('A', 17)));
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("ABC123")]
    [InlineData("abc123")]
    public void Accepts_reasonable_reg(string reg)
    {
        var v = new GetVehicleByRegValidator();
        var result = v.Validate(new GetVehicleByRegQuery(reg));
        result.IsValid.Should().BeTrue();
    }
}
