namespace Vehicles.Domain.ValueObjects;

public sealed record RegistrationNumber
{
    public string Value { get; }

    private RegistrationNumber(string value)
        => Value = value.Trim().ToUpperInvariant();

    public static RegistrationNumber From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new DomainException("Registration number is required.");
        return new RegistrationNumber(input);
    }

    public override string ToString() => Value;
}
