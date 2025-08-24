using Insurance.Application.Dtos;
using Insurance.Application.Ports;
using Insurance.Domain.ValueObjects;
using MediatR;

namespace Insurance.Application.UseCases.GetInsuranceSummary;

public class GetInsuranceSummaryHandler : IRequestHandler<GetInsuranceSummaryQuery, InsuranceSummaryDto>
{
    private readonly IInsuranceDataPort _insurance;
    private readonly IVehicleLookupPort _vehicles;

    public GetInsuranceSummaryHandler(IInsuranceDataPort insurance, IVehicleLookupPort vehicles)
    {
        _insurance = insurance;
        _vehicles = vehicles;
    }

    public async Task<InsuranceSummaryDto> Handle(GetInsuranceSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var policies = await _insurance.GetPoliciesAsync(request.PersonalNumber, cancellationToken);

        var carRegs = policies
            .Where(p => p.Type == PolicyType.Car && !string.IsNullOrWhiteSpace(p.VehicleRegNumber))
            .Select(p => p.VehicleRegNumber!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var vehiclesByReg = carRegs.Length == 0
            ? new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase)
            : (Dictionary<string, VehicleInfoDto>)await _vehicles.GetByRegsAsync(carRegs, cancellationToken);

        var summaries = new List<PolicySummaryDto>(policies.Count);
        foreach (var p in policies)
        {
            VehicleInfoDto? vehicleDto = null;
            if (p is { Type: PolicyType.Car, VehicleRegNumber: { } reg })
                vehiclesByReg.TryGetValue(reg, out vehicleDto);

            summaries.Add(new PolicySummaryDto(
                PolicyType: p.Type.ToString(),
                MonthlyCost: p.MonthlyCost.Amount,
                Currency: p.MonthlyCost.Currency,
                VehicleRegNumber: p.VehicleRegNumber,
                Vehicle: vehicleDto));
        }

        var currency = policies.Count > 0 ? policies[0].MonthlyCost.Currency : "USD";
        var total = policies.Select(p => p.MonthlyCost)
            .Aggregate(Money.Zero(currency), (acc, next) => acc + next);

        return new InsuranceSummaryDto(
            PersonalNumber: request.PersonalNumber,
            Policies: summaries,
            TotalMonthlyCost: total.Amount,
            Currency: total.Currency);
    }
}
