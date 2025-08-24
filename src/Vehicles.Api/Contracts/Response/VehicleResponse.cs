namespace Vehicles.Api.Contracts.Response;

public sealed record VehicleResponse(string RegNumber, string Make, string Model, int Year, string Vin);
