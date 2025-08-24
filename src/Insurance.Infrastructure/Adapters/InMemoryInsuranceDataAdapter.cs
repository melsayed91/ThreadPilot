using Insurance.Application.Ports;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;

namespace Insurance.Infrastructure.Adapters;

public class InMemoryInsuranceDataAdapter : IInsuranceDataPort
{
    private readonly Dictionary<string, List<InsurancePolicy>> _byPerson = new(StringComparer.OrdinalIgnoreCase)
    {
        ["19650101-1234"] =
        [
            new InsurancePolicy(PolicyType.Pet, Money.Usd(10)),
            new InsurancePolicy(PolicyType.PersonalHealth, Money.Usd(20)),
            new InsurancePolicy(PolicyType.Car, Money.Usd(30), "ABC123"),
            new InsurancePolicy(PolicyType.Car, Money.Usd(30), "ABC123"),
            new InsurancePolicy(PolicyType.Car, Money.Usd(30), "XYZ999")
        ],
        ["19700101-1111"] =
        [
            new InsurancePolicy(PolicyType.Pet, Money.Usd(10)),
            new InsurancePolicy(PolicyType.PersonalHealth, Money.Usd(20))
        ]
    };

    public Task<IReadOnlyList<InsurancePolicy>> GetPoliciesAsync(string personalNumber, CancellationToken ct)
        => Task.FromResult<IReadOnlyList<InsurancePolicy>>(
            _byPerson.TryGetValue(personalNumber, out var list) ? list : []);
}
