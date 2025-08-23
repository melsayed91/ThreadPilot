using MediatR;
using Vehicles.Application.Dtos;

namespace Vehicles.Application.UseCases.GetVehicleByReg;

public sealed record GetVehicleByRegQuery(string RegNumber) : IRequest<VehicleDto>;
