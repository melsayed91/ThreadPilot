using Insurance.Application.Ports;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Adapters;

public class InsuranceDataAdapter : IInsuranceDataPort
{
    private readonly InsuranceDbContext _db;
    public InsuranceDataAdapter(InsuranceDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

    public async Task<IReadOnlyList<InsurancePolicy>> GetPoliciesAsync(string personalNumber, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(personalNumber))
            throw new ArgumentException("Personal number is required.", nameof(personalNumber));

        var norm = personalNumber.Trim();

        var rows = await _db.Policies.AsNoTracking()
            .Where(p => p.PersonalNumber == norm)
            .OrderBy(p => p.Type)
            .ToListAsync(ct);

        return rows.Select(r => r.ToDomain()).ToList();
    }
}
