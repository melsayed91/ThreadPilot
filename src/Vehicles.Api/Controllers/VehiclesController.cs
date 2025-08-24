using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Vehicles.Api.Contracts.Request;
using Vehicles.Api.Contracts.Response;
using Vehicles.Application.UseCases.GetVehicleByReg;
using Vehicles.Application.UseCases.GetVehiclesBatch;
using Vehicles.Domain;

namespace Vehicles.Api.Controllers;

[ApiController]
[Route("/v1/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly ISender _sender;
    public VehiclesController(ISender sender) => _sender = sender;

    [HttpGet("{reg}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByReg([FromRoute] string reg, CancellationToken ct)
    {
        try
        {
            var dto = await _sender.Send(new GetVehicleByRegQuery(reg), ct);
            return Ok(new VehicleResponse(dto.RegNumber, dto.Make, dto.Model, dto.Year, dto.Vin));
        }
        catch (DomainException)
        {
            return NotFound(new ProblemDetails
                { Title = "Vehicle not found", Status = 404, Detail = $"Vehicle {reg} not found" });
        }
    }

    [HttpPost("batch")]
    [FeatureGate("EnableVehiclesBatchEndpoint")]
    [ProducesResponseType(typeof(VehicleBatchResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Batch([FromBody] VehicleBatchRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);
        var dtos = await _sender.Send(new GetVehiclesBatchQuery(req.RegNumbers), ct);
        var list = dtos.Select(d => new VehicleResponse(d.RegNumber, d.Make, d.Model, d.Year, d.Vin));
        return Ok(new VehicleBatchResponse(list));
    }
}
