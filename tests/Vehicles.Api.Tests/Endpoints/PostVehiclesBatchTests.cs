using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Api.Contracts.Request;
using Vehicles.Api.Contracts.Response;


namespace Vehicles.Api.Tests.Endpoints;

public class PostVehiclesBatchTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public PostVehiclesBatchTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Returns_200_and_deduplicates_results()
    {
        var client = _factory.CreateClient();

        var payload = new VehicleBatchRequest(["abc123", "ABC123", "xyz999"]);
        var res = await client.PostAsJsonAsync("/v1/vehicles/batch", payload);

        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await res.Content.ReadFromJsonAsync<VehicleBatchResponse>();
        body!.Vehicles.Should().HaveCount(2);
        body.Vehicles.Select(v => v.RegNumber).Should().BeEquivalentTo("ABC123", "XYZ999");
    }

    [Fact]
    public async Task Returns_400_when_payload_invalid()
    {
        var client = _factory.CreateClient();

        var res = await client.PostAsJsonAsync("/v1/vehicles/batch",
            new VehicleBatchRequest(Array.Empty<string>()));

        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await res.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(400);
        problem.Title.Should().ContainEquivalentOf("validation");
    }
}
