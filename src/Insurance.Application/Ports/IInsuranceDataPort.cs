using Insurance.Domain.Entities;

namespace Insurance.Application.Ports;

public interface IInsuranceDataPort
{
    Task<IReadOnlyList<Policy>> GetPoliciesAsync(string personalNumber, CancellationToken ct);
}
