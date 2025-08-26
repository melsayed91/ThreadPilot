using Insurance.Application.Ports;
using Insurance.Domain.Entities;
using Insurance.Domain.ValueObjects;

namespace Insurance.Infrastructure.Adapters;

public class InMemoryInsuranceDataAdapter : IInsuranceDataPort
{
    private readonly Dictionary<string, List<InsurancePolicy>> _byPerson =
        new(StringComparer.OrdinalIgnoreCase);

    public InMemoryInsuranceDataAdapter()
    {
        Seed("19650101-1234", [
            Make(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "19650101-1234", PolicyType.Pet, Money.Usd(10)),
            Make(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "19650101-1234", PolicyType.PersonalHealth,
                Money.Usd(20)),
            Make(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "19650101-1234", PolicyType.Car, Money.Usd(30),
                "ABC123"),
            Make(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "19650101-1234", PolicyType.Car, Money.Usd(30),
                "XYZ999")
        ]);

        Seed("19700101-1111", [
            Make(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "19700101-1111", PolicyType.Pet, Money.Usd(10)),
            Make(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "19700101-1111", PolicyType.PersonalHealth,
                Money.Usd(20))
        ]);
    }

    private static InsurancePolicy Make(Guid id, string pn, PolicyType type, Money cost, string? reg = null) =>
        new(id, PersonalNumber.From(pn), type, cost, reg);

    private void Seed(string pn, IEnumerable<InsurancePolicy> policies)
        => _byPerson[PersonalNumber.From(pn).Value] = policies.ToList();

    public Task<IReadOnlyList<InsurancePolicy>> GetPoliciesAsync(string personalNumber, CancellationToken ct)
    {
        var key = PersonalNumber.From(personalNumber).Value;
        return Task.FromResult<IReadOnlyList<InsurancePolicy>>(
            _byPerson.TryGetValue(key, out var list) ? list : []);
    }
}
