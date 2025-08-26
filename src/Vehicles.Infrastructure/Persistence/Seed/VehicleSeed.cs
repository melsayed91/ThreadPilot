using Vehicles.Infrastructure.Persistence.Entities;

namespace Vehicles.Infrastructure.Persistence.Seed;

internal static class VehicleSeed
{
    public static readonly VehicleEntity[] Initial =
    [
        new()
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            RegistrationNumber = "ABC123",
            Make = "Tesla",
            Model = "Model 3",
            Year = 2020,
            Vin = "VIN-A"
        },
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            RegistrationNumber = "XYZ999",
            Make = "Volvo",
            Model = "XC90",
            Year = 2019,
            Vin = "VIN-X"
        },
        new()
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            RegistrationNumber = "KLM456",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2018,
            Vin = "VIN-K"
        }
    ];
}
