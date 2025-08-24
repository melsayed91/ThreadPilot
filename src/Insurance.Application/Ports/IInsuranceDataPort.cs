using Insurance.Domain.Entities;

namespace Insurance.Application.Ports;

public interface IInsuranceDataPort
{
    Task<IReadOnlyList<InsurancePolicy>> GetPoliciesAsync(string personalNumber, CancellationToken ct);
}
