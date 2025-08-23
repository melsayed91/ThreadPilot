using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Application.Ports;

public interface IVehicleDataSource
{
    Task<Vehicle?> GetByRegAsync(RegistrationNumber reg, CancellationToken ct);
    Task<IDictionary<string, Vehicle>> GetByRegsAsync(IEnumerable<RegistrationNumber> regs, CancellationToken ct);
}
