using FluentAssertions;
using Vehicles.Application.UseCases.GetVehiclesBatch;

namespace Vehicles.Application.Tests.UseCases.GetVehiclesBatch;

public class GetVehiclesBatchValidatorTests
{
    [Fact]
    public void Rejects_empty_collection()
    {
        var v = new GetVehiclesBatchValidator();
        var result = v.Validate(new GetVehiclesBatchQuery(Array.Empty<string>()));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Rejects_item_with_empty_reg()
    {
        var v = new GetVehiclesBatchValidator();
        var result = v.Validate(new GetVehiclesBatchQuery(["ABC123", "   "]));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Accepts_valid_list()
    {
        var v = new GetVehiclesBatchValidator();
        var result = v.Validate(new GetVehiclesBatchQuery(["ABC123", "XYZ999"]));
        result.IsValid.Should().BeTrue();
    }
}
