using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Insurance.Application.Dtos;
using Insurance.Infrastructure.Adapters;

namespace Insurance.Infrastructure.Tests.Adapters;

public class VehicleLookupHttpAdapterTests
{
    [Fact]
    public async Task Batch_returns_dictionary_by_reg()
    {
        var handler = new FakeHandler(async (request, ct) =>
        {
            if (request.Method != HttpMethod.Post || request.RequestUri!.AbsolutePath != "/v1/vehicles/batch")
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            var payload = await request.Content!.ReadFromJsonAsync<RegPayload>(cancellationToken: ct)
                          ?? new RegPayload([]);

            var vehicles = payload.RegNumbers.Select(r =>
                    new VehicleInfoDto(r.ToUpperInvariant(), "Make", "Model", 2020, $"VIN-{r.ToUpperInvariant()}"))
                .ToArray();

            var json = JsonSerializer.Serialize(new { vehicles });
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        });

        var http = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        var adapter = new VehicleLookupHttpAdapter(http);

        // Act
        var dict = await adapter.GetByRegsAsync(["abc123", "xyz999"], CancellationToken.None);

        // Assert
        dict.Should().ContainKeys("ABC123", "XYZ999");
        dict["ABC123"].Vin.Should().Be("VIN-ABC123");
    }

    private sealed record RegPayload(string[] RegNumbers);

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _fn;
        public FakeHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> fn) => _fn = fn;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
            => _fn(request, cancellationToken);
    }
}
