namespace Vehicles.Application.Dtos;

public sealed record VehicleDto(string RegNumber, string Make, string Model, int Year, string Vin);
