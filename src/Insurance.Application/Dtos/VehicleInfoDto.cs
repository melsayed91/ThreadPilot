namespace Insurance.Application.Dtos;

public sealed record VehicleInfoDto(
    string RegNumber,
    string Make,
    string Model,
    int Year,
    string Vin);
