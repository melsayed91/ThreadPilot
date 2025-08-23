using Insurance.Application.Ports;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;

namespace Insurance.Infrastructure.Adapters;

public class InMemoryInsuranceDataAdapter : IInsuranceDataPort
{
    private readonly Dictionary<string, List<Policy>> _byPerson = new(StringComparer.OrdinalIgnoreCase)
    {
        ["19650101-1234"] =
        [
            new Policy(PolicyType.Pet, Money.Usd(10)),
            new Policy(PolicyType.PersonalHealth, Money.Usd(20)),
            new Policy(PolicyType.Car, Money.Usd(30), "ABC123"),
            new Policy(PolicyType.Car, Money.Usd(30), "ABC123"),
            new Policy(PolicyType.Car, Money.Usd(30), "XYZ999")
        ],
        ["19700101-1111"] =
        [
            new Policy(PolicyType.Pet, Money.Usd(10)),
            new Policy(PolicyType.PersonalHealth, Money.Usd(20))
        ]
    };

    public Task<IReadOnlyList<Policy>> GetPoliciesAsync(string personalNumber, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<Policy>>(
            _byPerson.TryGetValue(personalNumber, out var list) ? list : []);
}
