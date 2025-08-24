using FluentAssertions;
using Insurance.Application.Dtos;
using Insurance.Application.Ports;
using Insurance.Application.UseCases.GetInsuranceSummary;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;
using Moq;

namespace Insurance.Application.Tests.UseCases.GetInsuranceSummary;

public class GetInsuranceSummaryHandlerTests
{
    [Fact]
    public async Task Enriches_car_policies_batches_once_and_sums_totals()
    {
        var policies = new List<InsurancePolicy>
        {
            new(PolicyType.Pet, Money.Usd(10)),
            new(PolicyType.PersonalHealth, Money.Usd(20)),
            new(PolicyType.Car, Money.Usd(30), "ABC123"),
            new(PolicyType.Car, Money.Usd(30), "ABC123"),
            new(PolicyType.Car, Money.Usd(30), "XYZ999"),
        };

        var insurancePort = new Mock<IInsuranceDataPort>();
        insurancePort.Setup(p => p.GetPoliciesAsync("19650101-1234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(policies);

        var vehiclesPort = new Mock<IVehicleLookupPort>();
        vehiclesPort.Setup(v => v.GetByRegsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase)
            {
                ["ABC123"] = new("ABC123", "Tesla", "Model 3", 2020, "VIN-A"),
                ["XYZ999"] = new("XYZ999", "Volvo", "XC90", 2019, "VIN-X")
            });

        var handler = new GetInsuranceSummaryHandler(insurancePort.Object, vehiclesPort.Object);

        var dto = await handler.Handle(new GetInsuranceSummaryQuery("19650101-1234"), CancellationToken.None);

        vehiclesPort.Verify(v => v.GetByRegsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()),
            Times.Once);

        dto.TotalMonthlyCost.Should().Be(10 + 20 + 30 + 30 + 30);
        dto.Currency.Should().Be("USD");
        dto.Policies.Should().HaveCount(5);

        var carSummaries = dto.Policies.Where(p => p.PolicyType == "Car").ToList();
        carSummaries.Should().HaveCount(3);
        carSummaries.Select(p => p.VehicleRegNumber).Distinct(StringComparer.OrdinalIgnoreCase)
            .Should().BeEquivalentTo("ABC123", "XYZ999");

        carSummaries.Should().OnlyContain(p => p.Vehicle != null);
        dto.Policies.Where(p => p.PolicyType != "Car").Should().OnlyContain(p => p.Vehicle == null);
    }

    [Fact]
    public async Task Returns_policies_without_vehicle_enrichment_when_no_car_policies()
    {
        var policies = new List<InsurancePolicy>
        {
            new(PolicyType.Pet, Money.Usd(10)),
            new(PolicyType.PersonalHealth, Money.Usd(20))
        };

        var insurancePort = new Mock<IInsuranceDataPort>();
        insurancePort.Setup(p => p.GetPoliciesAsync("19700101-1111", It.IsAny<CancellationToken>()))
            .ReturnsAsync(policies);

        var vehiclesPort = new Mock<IVehicleLookupPort>();
        vehiclesPort.Setup(v => v.GetByRegsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .Throws(new InvalidOperationException("Should not be called"));

        var handler = new GetInsuranceSummaryHandler(insurancePort.Object, vehiclesPort.Object);

        var dto = await handler.Handle(new GetInsuranceSummaryQuery("19700101-1111"), CancellationToken.None);

        dto.TotalMonthlyCost.Should().Be(30);
        dto.Policies.Should().HaveCount(2);
        dto.Policies.Should().OnlyContain(p => p.Vehicle == null);
    }
}
