using FluentAssertions;
using Insurance.Domain.ValueObjects;

namespace Insurance.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Add_SameCurrency_SumsAmounts()
    {
        var a = Money.Usd(10);
        var b = Money.Usd(20);
        (a + b).Should().Be(Money.Usd(30));
    }

    [Fact]
    public void Add_DifferentCurrency_Throws()
    {
        var a = new Money(10, "USD");
        var b = new Money(5, "EUR");
        var act = () =>
        {
            var _ = a + b;
        };
        act.Should().Throw<DomainException>();
    }
}
