using Insurance.Application.Dtos;

namespace Insurance.Application.Ports;

public interface IVehicleLookupPort
{
    Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct);
    Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs, CancellationToken ct);
}
