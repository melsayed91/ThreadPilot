using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;
using Insurance.Infrastructure.Persistence.Entities;

namespace Insurance.Infrastructure.Persistence.Mappers;

internal static class PolicyMapper
{
    public static InsurancePolicy ToDomain(this InsurancePolicyEntity row)
        => new(
            id: row.Id,
            personalNumber: PersonalNumber.From(row.PersonalNumber),
            type: (PolicyType)row.Type,
            monthlyCost: new Money(row.MonthlyCostAmount, row.MonthlyCostCurrency),
            vehicleRegNumber: row.VehicleRegNumber
        );

    public static InsurancePolicyEntity ToEntity(this InsurancePolicy domain, string personalNumber, Guid? id = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            PersonalNumber = personalNumber,
            Type = (int)domain.Type,
            MonthlyCostAmount = domain.MonthlyCost.Amount,
            MonthlyCostCurrency = domain.MonthlyCost.Currency,
            VehicleRegNumber = domain.VehicleRegNumber
        };
}
