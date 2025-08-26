using System.Text.RegularExpressions;

namespace Insurance.Domain.ValueObjects;

public sealed record PersonalNumber
{
    public string Value { get; }
    private PersonalNumber(string value) => Value = value;

    public static PersonalNumber From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new DomainException("Personal number is required.");

        var v = input.Trim();

        if (!Regex.IsMatch(v, @"^\d{8}-\d{4}$"))
            throw new DomainException("Invalid personal number format. Use YYYYMMDD-XXXX.");

        return new PersonalNumber(v);
    }

    public override string ToString() => Value;
}
