using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Insurance.Api.Tests.Fakes;
using Insurance.Application.Dtos;
using Insurance.Application.Ports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.Tests.Endpoints;

public class GetInsuranceSummaryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _baseFactory;

    public GetInsuranceSummaryTests(WebApplicationFactory<Program> factory) => _baseFactory = factory;

    private (WebApplicationFactory<Program> Factory, FakeVehicleLookupPort Fake) WithVehiclesStub()
    {
        var dict = new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase)
        {
            ["ABC123"] = new("ABC123", "Tesla", "Model 3", 2020, "VIN-A"),
            ["XYZ999"] = new("XYZ999", "Volvo", "XC90", 2019, "VIN-X")
        };
        var fake = new FakeVehicleLookupPort(dict);

        var factory = _baseFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var toRemove = services.Where(sd => sd.ServiceType == typeof(IVehicleLookupPort)).ToList();
                foreach (var sd in toRemove) services.Remove(sd);

                services.AddSingleton<IVehicleLookupPort>(fake);
            });
        });

        return (factory, fake);
    }

    [Fact]
    public async Task Returns_200_with_enriched_car_policies_and_sums_totals()
    {
        var (factory, fake) = WithVehiclesStub();
        var client = factory.CreateClient();

        var res = await client.GetAsync("/v1/insurances/19650101-1234");

        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await res.Content.ReadFromJsonAsync<InsuranceSummaryDto>();
        dto!.TotalMonthlyCost.Should().Be(10 + 20 + 30 + 30 + 30);
        dto.Currency.Should().Be("USD");

        var cars = dto.Policies.Where(p => p.PolicyType == "Car").ToList();
        cars.Should().HaveCount(3);
        cars.Should().OnlyContain(p => p.Vehicle != null);
        cars.Select(p => p.VehicleRegNumber).Distinct(StringComparer.OrdinalIgnoreCase)
            .Should().BeEquivalentTo(new[] { "ABC123", "XYZ999" });

        fake.BatchCalls.Should().Be(1);
    }

    [Fact]
    public async Task Returns_400_for_invalid_personal_number()
    {
        var (factory, _) = WithVehiclesStub();
        var client = factory.CreateClient();

        var res = await client.GetAsync("/v1/insurances/19650101"); // bad format
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await res.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Title.Should().ContainEquivalentOf("validation");
    }

    [Fact]
    public async Task Returns_502_when_upstream_fails()
    {
        var throwingFake = new ThrowingVehicleLookup();
        var factory = _baseFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var toRemove = services.Where(sd => sd.ServiceType == typeof(IVehicleLookupPort)).ToList();
                foreach (var sd in toRemove) services.Remove(sd);
                services.AddSingleton<IVehicleLookupPort>(throwingFake);
            });
        });

        var client = factory.CreateClient();
        var res = await client.GetAsync("/v1/insurances/19650101-1234");

        res.StatusCode.Should().Be(HttpStatusCode.BadGateway);
        var problem = await res.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Title.Should().ContainEquivalentOf("upstream");
    }

    private sealed class ThrowingVehicleLookup : IVehicleLookupPort
    {
        public Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct)
            => throw new HttpRequestException("Vehicles service unavailable");

        public Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs,
            CancellationToken ct)
            => throw new HttpRequestException("Vehicles service unavailable");
    }
}
