using FluentAssertions;
using Insurance.Infrastructure.Adapters;

namespace Insurance.Infrastructure.Tests.Adapters;

public class InMemoryInsuranceDataAdapterTests
{
    [Fact]
    public async Task Returns_seeded_policies_per_person()
    {
        var adapter = new InMemoryInsuranceDataAdapter();

        var a = await adapter.GetPoliciesAsync("19650101-1234", CancellationToken.None);
        var b = await adapter.GetPoliciesAsync("unknown", CancellationToken.None);

        a.Should().HaveCountGreaterThan(0);
        b.Should().BeEmpty();
    }
}
