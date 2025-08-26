namespace Vehicles.Infrastructure.Persistence.Entities;

public sealed class VehicleEntity
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; } = null!;
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string Vin { get; set; } = null!;
}
