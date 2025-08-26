using Vehicles.Domain.Entities;
using Vehicles.Domain.ValueObjects;
using Vehicles.Infrastructure.Persistence.Entities;

namespace Vehicles.Infrastructure.Persistence.Mappers;

internal static class VehicleMapper
{
    public static Vehicle ToDomain(this VehicleEntity row)
        => new(
            id: row.Id,
            regNumber: RegistrationNumber.From(row.RegistrationNumber),
            make: row.Make,
            model: row.Model,
            year: row.Year,
            vin: row.Vin);

    public static VehicleEntity ToEntity(this Vehicle entity)
        => new()
        {
            Id = entity.Id,
            RegistrationNumber = entity.RegNumber.Value,
            Make = entity.Make,
            Model = entity.Model,
            Year = entity.Year,
            Vin = entity.Vin
        };
}
