namespace Insurance.Domain.ValueObjects;

public sealed record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public static Money operator +(Money a, Money b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        
        if (!string.Equals(a.Currency, b.Currency, StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Currency mismatch.");
        return a with { Amount = a.Amount + b.Amount };
    }

    public override string ToString() => $"{Amount:0.##} {Currency.ToUpperInvariant()}";

    public static Money Usd(decimal amount) => new(amount, "USD");

    public static Money Add(Money a, Money b) => a + b;
}
