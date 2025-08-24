using Insurance.Application.Dtos;
using Insurance.Application.Ports;
using Microsoft.FeatureManagement;

namespace Insurance.Infrastructure.Decorators;

public class FeatureGatedVehicleLookup : IVehicleLookupPort
{
    private readonly IVehicleLookupPort _inner;
    private readonly IFeatureManagerSnapshot _features;

    public FeatureGatedVehicleLookup(IVehicleLookupPort inner, IFeatureManagerSnapshot features)
    {
        _inner = inner;
        _features = features;
    }

    public async Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct)
    {
        if (!await _features.IsEnabledAsync("EnableInsuranceVehicleEnrichment")) return null;
        return await _inner.GetByRegAsync(reg, ct);
    }

    public async Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs,
        CancellationToken ct)
    {
        if (!await _features.IsEnabledAsync("EnableInsuranceVehicleEnrichment"))
            return new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase);

        return await _inner.GetByRegsAsync(regs, ct);
    }
}
