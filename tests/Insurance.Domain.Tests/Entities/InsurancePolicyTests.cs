using FluentAssertions;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Tests.Entities;

public class InsurancePolicyTests
{
    private static readonly PersonalNumber Pn = PersonalNumber.From("19650101-1234");

    [Fact]
    public void CarPolicy_WithoutReg_Throws()
    {
        var act = () => new InsurancePolicy(Guid.NewGuid(), Pn, PolicyType.Car, Money.Usd(30));
        act.Should().Throw<DomainException>()
            .WithMessage("*requires a vehicle registration number*");
    }

    [Fact]
    public void PetPolicy_NoReg_Allowed()
    {
        var p = new InsurancePolicy(Guid.NewGuid(), Pn, PolicyType.Pet, Money.Usd(10));
        p.VehicleRegNumber.Should().BeNull();
        p.MonthlyCost.Amount.Should().Be(10);
    }

    [Fact]
    public void UnknownPolicyType_IsRejected()
    {
        var act = () => new InsurancePolicy(Guid.NewGuid(), Pn, PolicyType.Unknown, Money.Usd(10));
        act.Should().Throw<DomainException>()
            .WithMessage("*Invalid policy type*");
    }
}
