using FluentAssertions;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Tests.Entities;

public class PolicyTests
{
    [Fact]
    public void CarPolicy_WithoutReg_Throws()
    {
        var act = () => new Policy(PolicyType.Car, Money.Usd(30));
        act.Should().Throw<DomainException>()
            .WithMessage("*requires a vehicle registration number*");
    }

    [Fact]
    public void PetPolicy_NoReg_Allowed()
    {
        var p = new Policy(PolicyType.Pet, Money.Usd(10));
        p.VehicleRegNumber.Should().BeNull();
        p.MonthlyCost.Amount.Should().Be(10);
    }

    [Fact]
    public void UnknownPolicyType_IsRejected()
    {
        var act = () => new Policy(PolicyType.Unknown, Money.Usd(10));
        act.Should().Throw<DomainException>()
            .WithMessage("*Invalid policy type*");
    }
}
