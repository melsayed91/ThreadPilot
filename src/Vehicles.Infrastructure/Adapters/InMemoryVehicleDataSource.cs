using Vehicles.Application.Ports;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Infrastructure.Adapters;

public class InMemoryVehicleDataSource : IVehicleDataSource
{
    private readonly Dictionary<string, Vehicle> _byReg;

    public InMemoryVehicleDataSource(IEnumerable<Vehicle>? seed = null)
    {
        _byReg = new Dictionary<string, Vehicle>(StringComparer.OrdinalIgnoreCase);

        var vehicles = seed?.ToList() ?? [];

        if (vehicles.Count == 0)
        {
            vehicles.AddRange([
                new Vehicle(RegistrationNumber.From("ABC123"), "Tesla", "Model 3", 2020, "VIN-A"),
                new Vehicle(RegistrationNumber.From("XYZ999"), "Volvo", "XC90", 2019, "VIN-X"),
                new Vehicle(RegistrationNumber.From("KLM456"), "Toyota", "Corolla", 2018, "VIN-K")
            ]);
        }

        foreach (var v in vehicles)
            _byReg[v.RegNumber.Value] = v;
    }

    public Task<Vehicle?> GetByRegAsync(RegistrationNumber reg, CancellationToken ct)
        => Task.FromResult(_byReg.GetValueOrDefault(reg.Value));

    public Task<IDictionary<string, Vehicle>> GetByRegsAsync(IEnumerable<RegistrationNumber> regs, CancellationToken ct)
    {
        var map = new Dictionary<string, Vehicle>(StringComparer.OrdinalIgnoreCase);
        foreach (var r in regs)
        {
            if (_byReg.TryGetValue(r.Value, out var v))
                map[r.Value] = v;
        }

        return Task.FromResult<IDictionary<string, Vehicle>>(map);
    }
}
