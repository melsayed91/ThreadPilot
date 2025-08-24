namespace Vehicles.Api.Contracts.Response;

public sealed record VehicleBatchResponse(IEnumerable<VehicleResponse> Vehicles);
