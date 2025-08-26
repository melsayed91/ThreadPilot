using Microsoft.EntityFrameworkCore;
using Vehicles.Application.Ports;
using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;
using Vehicles.Infrastructure.Persistence;
using Vehicles.Infrastructure.Persistence.Mappers;

namespace Vehicles.Infrastructure.Adapters;

public sealed class VehicleDataSource : IVehicleDataSource
{
    private readonly VehiclesDbContext _db;

    public VehicleDataSource(VehiclesDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<Vehicle?> GetByRegAsync(RegistrationNumber reg, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(reg);

        var row = await _db.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.RegistrationNumber == reg.Value, ct);

        return row?.ToDomain();
    }

    public async Task<IDictionary<string, Vehicle>> GetByRegsAsync(IEnumerable<RegistrationNumber> regs,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(regs);

        var keys = regs.Select(r => r.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        if (keys.Length == 0)
            return new Dictionary<string, Vehicle>(StringComparer.OrdinalIgnoreCase);

        var rows = await _db.Vehicles
            .AsNoTracking()
            .Where(v => keys.Contains(v.RegistrationNumber))
            .ToListAsync(ct);

        return rows.ToDictionary(r => r.RegistrationNumber, r => r.ToDomain(), StringComparer.OrdinalIgnoreCase);
    }
}
