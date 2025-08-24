namespace Vehicles.Api.Contracts.Request;

public sealed record VehicleBatchRequest(IEnumerable<string> RegNumbers);
