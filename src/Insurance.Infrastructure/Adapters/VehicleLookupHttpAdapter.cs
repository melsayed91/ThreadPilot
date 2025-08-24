using System.Net;
using System.Net.Http.Json;
using Insurance.Application.Dtos;
using Insurance.Application.Ports;

namespace Insurance.Infrastructure.Adapters;

public class VehicleLookupHttpAdapter : IVehicleLookupPort
{
    private readonly HttpClient _http;
    public VehicleLookupHttpAdapter(HttpClient http) => _http = http;

    public async Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct)
    {
        var uri = new Uri($"/v1/vehicles/{Uri.EscapeDataString(reg)}", UriKind.Relative);
        using var res = await _http.GetAsync(uri, ct);
        if (res.StatusCode == HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<VehicleInfoDto>(cancellationToken: ct);
    }

    public async Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs,
        CancellationToken ct)
    {
        var payload = new { regNumbers = regs.Distinct(StringComparer.OrdinalIgnoreCase).ToArray() };

        using var res = await _http.PostAsJsonAsync("/v1/vehicles/batch", payload, ct);
        res.EnsureSuccessStatusCode();

        var wrapper = await res.Content.ReadFromJsonAsync<VehicleBatchWrapper>(cancellationToken: ct)
                      ?? new VehicleBatchWrapper(Array.Empty<VehicleInfoDto>());

        var dict = wrapper.Vehicles.ToDictionary(v => v.RegNumber, v => v, StringComparer.OrdinalIgnoreCase);
        return dict;
    }

    private sealed record VehicleBatchWrapper(IEnumerable<VehicleInfoDto> Vehicles);
}
