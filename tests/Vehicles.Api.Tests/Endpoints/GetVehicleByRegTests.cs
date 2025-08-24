using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Vehicles.Api.Contracts.Response;

namespace Vehicles.Api.Tests.Endpoints;

public class GetVehicleByRegTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public GetVehicleByRegTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Returns_200_with_vehicle_when_found()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync(new Uri("/v1/vehicles/ABC123", UriKind.Relative));

        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await res.Content.ReadFromJsonAsync<VehicleResponse>();
        body!.RegNumber.Should().Be("ABC123");
    }

    [Fact]
    public async Task Returns_404_when_not_found()
    {
        var client = _factory.CreateClient();
        var res = await client.GetAsync(new Uri("/v1/vehicles/NOPE999", UriKind.Relative));

        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await res.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(404);
        problem.Title.Should().ContainEquivalentOf("not found");
    }
}
