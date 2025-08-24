using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Entities;

public sealed class InsurancePolicy
{
    public PolicyType Type { get; }
    public Money MonthlyCost { get; }
    public string? VehicleRegNumber { get; }

    public InsurancePolicy(PolicyType type, Money monthlyCost, string? vehicleRegNumber = null)
    {
        if (!Enum.IsDefined(typeof(PolicyType), type) || type == PolicyType.Unknown)
            throw new DomainException("Invalid policy type.");

        if (type == PolicyType.Car && string.IsNullOrWhiteSpace(vehicleRegNumber))
            throw new DomainException("Car insurance requires a vehicle registration number.");

        Type = type;
        MonthlyCost = monthlyCost;
        VehicleRegNumber = vehicleRegNumber;
    }
}
