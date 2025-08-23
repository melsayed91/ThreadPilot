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
        using var res = await _http.GetAsync($"/v1/vehicles/{Uri.EscapeDataString(reg)}", ct);
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

        var body = await res.Content.ReadFromJsonAsync<VehicleInfoDto[]>(cancellationToken: ct)
                   ?? [];

        return body.ToDictionary(v => v.RegNumber, v => v, StringComparer.OrdinalIgnoreCase);
    }
}
