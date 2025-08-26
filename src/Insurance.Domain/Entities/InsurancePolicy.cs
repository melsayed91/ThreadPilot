using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Entities;

public sealed class InsurancePolicy
{
    public Guid Id { get; }
    public PersonalNumber PersonalNumber { get; }
    public PolicyType Type { get; }
    public Money MonthlyCost { get; }
    public string? VehicleRegNumber { get; }

    public InsurancePolicy(Guid id,
        PersonalNumber personalNumber, PolicyType type, Money monthlyCost, string? vehicleRegNumber = null)
    {
        if (id == Guid.Empty) throw new DomainException("Id is required.");
        ArgumentNullException.ThrowIfNull(personalNumber);
        ArgumentNullException.ThrowIfNull(monthlyCost);
        if (!Enum.IsDefined(type) || type == PolicyType.Unknown)
            throw new DomainException("Invalid policy type.");

        if (type == PolicyType.Car && string.IsNullOrWhiteSpace(vehicleRegNumber))
            throw new DomainException("Car insurance requires a vehicle registration number.");

        Id = id;
        PersonalNumber = personalNumber;
        Type = type;
        MonthlyCost = monthlyCost;
        VehicleRegNumber = vehicleRegNumber;
    }
}
