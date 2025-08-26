namespace Vehicles.Domain.ValueObjects;

public sealed record RegistrationNumber
{
    public string Value { get; private init; } = null!;

    private RegistrationNumber()
    {
    }

    public static RegistrationNumber From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new DomainException("Registration number is required.");
        return new RegistrationNumber
        {
            Value = input.Trim().ToUpperInvariant()
        };
    }

    public override string ToString() => Value;
}
