using FluentAssertions;
using Vehicles.Domain.ValueObjects;

namespace Vehicles.Domain.Tests.ValueObjects;

public class RegistrationNumberTests
{
    [Fact]
    public void From_Uppercases_And_Trims()
    {
        var rn = RegistrationNumber.From("  abC123  ");
        rn.ToString().Should().Be("ABC123");
    }

    [Fact]
    public void From_Empty_Throws()
    {
        var act = () => RegistrationNumber.From("   ");
        act.Should().Throw<DomainException>();
    }
}
