using MediatR;
using Vehicles.Application.Dtos;
using Vehicles.Application.Ports;
using Vehicles.Domain;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Application.UseCases.GetVehicleByReg;

public class GetVehicleByRegHandler : IRequestHandler<GetVehicleByRegQuery, VehicleDto>
{
    private readonly IVehicleDataSource _source;

    public GetVehicleByRegHandler(IVehicleDataSource source) => _source = source;

    public async Task<VehicleDto> Handle(GetVehicleByRegQuery request, CancellationToken cancellationToken)
    {
        var reg = RegistrationNumber.From(request.RegNumber);
        var vehicle = await _source.GetByRegAsync(reg, cancellationToken);
        if (vehicle is null) throw new DomainException($"Vehicle {reg} not found");

        return new VehicleDto(
            vehicle.RegNumber.Value,
            vehicle.Make,
            vehicle.Model,
            vehicle.Year,
            vehicle.Vin);
    }
}
