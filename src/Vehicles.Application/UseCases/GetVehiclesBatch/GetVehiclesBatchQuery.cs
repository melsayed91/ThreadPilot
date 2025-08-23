using MediatR;
using Vehicles.Application.Dtos;

namespace Vehicles.Application.UseCases.GetVehiclesBatch;

public sealed record GetVehiclesBatchQuery(IEnumerable<string> RegNumbers) : IRequest<IReadOnlyList<VehicleDto>>;
