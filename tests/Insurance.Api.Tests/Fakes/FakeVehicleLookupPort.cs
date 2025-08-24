using Insurance.Application.Dtos;
using Insurance.Application.Ports;

namespace Insurance.Api.Tests.Fakes;

public class FakeVehicleLookupPort : IVehicleLookupPort
{
    private readonly IReadOnlyDictionary<string, VehicleInfoDto> _data;
    public int BatchCalls { get; private set; }

    public FakeVehicleLookupPort(IReadOnlyDictionary<string, VehicleInfoDto> data)
        => _data = data;

    public Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct)
        => Task.FromResult(_data.TryGetValue(reg, out var v) ? v : null);

    public Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs,
        CancellationToken ct)
    {
        BatchCalls++;
        var set = new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase);
        foreach (var r in regs.Distinct(StringComparer.OrdinalIgnoreCase))
            if (_data.TryGetValue(r, out var v))
                set[r] = v;
        return Task.FromResult((IReadOnlyDictionary<string, VehicleInfoDto>)set);
    }
}
