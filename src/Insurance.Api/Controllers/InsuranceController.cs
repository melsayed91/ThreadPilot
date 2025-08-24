using Insurance.Application.UseCases.GetInsuranceSummary;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("v1/insurances")]
public class InsuranceController : ControllerBase
{
    private readonly ISender _sender;
    public InsuranceController(ISender sender) => _sender = sender;

    [HttpGet("{personalNumber}")]
    [ProducesResponseType(typeof(Application.Dtos.InsuranceSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetSummary([FromRoute] string personalNumber, CancellationToken ct)
    {
        var dto = await _sender.Send(new GetInsuranceSummaryQuery(personalNumber), ct);
        return Ok(dto);
    }
}
