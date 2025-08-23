using MediatR;
using Vehicles.Application.Dtos;
using Vehicles.Application.Ports;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Application.UseCases.GetVehiclesBatch;

public class GetVehiclesBatchHandler : IRequestHandler<GetVehiclesBatchQuery, IReadOnlyList<VehicleDto>>
{
    private readonly IVehicleDataSource _source;

    public GetVehiclesBatchHandler(IVehicleDataSource source) => _source = source;

    public async Task<IReadOnlyList<VehicleDto>> Handle(GetVehiclesBatchQuery request, CancellationToken ct)
    {
        var regs = request.RegNumbers
            .Select(RegistrationNumber.From)
            .DistinctBy(r => r.Value)
            .ToArray();

        var map = await _source.GetByRegsAsync(regs, ct);

        return map.Values
            .Select(v => new VehicleDto(v.RegNumber.Value, v.Make, v.Model, v.Year, v.Vin))
            .ToList();
    }
}
