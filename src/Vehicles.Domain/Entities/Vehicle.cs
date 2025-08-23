using Vehicles.Domain.ValueObjects;

namespace Vehicles.Domain.Entities;

public sealed class Vehicle
{
    public RegistrationNumber RegNumber { get; }
    public string Make { get; }
    public string Model { get; }
    public int Year { get; }
    public string Vin { get; }

    public Vehicle(RegistrationNumber regNumber, string make, string model, int year, string vin)
    {
        RegNumber = regNumber ?? throw new DomainException("Registration number is required.");
        Make = string.IsNullOrWhiteSpace(make) ? throw new DomainException("Make is required.") : make.Trim();
        Model = string.IsNullOrWhiteSpace(model) ? throw new DomainException("Model is required.") : model.Trim();
        Year = year;
        Vin = string.IsNullOrWhiteSpace(vin) ? throw new DomainException("VIN is required.") : vin.Trim();
    }

    public override string ToString() => $"{RegNumber} {Make} {Model} ({Year})";
}
