using System.Text;
using Insurance.Application.Dtos;
using Insurance.Application.Ports;
using Insurance.Infrastructure.Concurrency;

namespace Insurance.Infrastructure.Decorators;

public sealed class CoalescingVehicleLookupAdapter : IVehicleLookupPort
{
    private readonly IVehicleLookupPort _inner;
    private readonly SingleFlightCoordinator _sf;

    public CoalescingVehicleLookupAdapter(IVehicleLookupPort inner, SingleFlightCoordinator sf)
    {
        _inner = inner;
        _sf = sf;
    }

    public Task<VehicleInfoDto?> GetByRegAsync(string reg, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(reg))
            return Task.FromResult<VehicleInfoDto?>(null);

        var key = $"reg:{Normalize(reg)}";
        return _sf.RunAsync(key, _ => _inner.GetByRegAsync(reg, ct), ct);
    }

    public Task<IReadOnlyDictionary<string, VehicleInfoDto>> GetByRegsAsync(IEnumerable<string> regs,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(regs);
        var regsList = regs.ToList();
        var canonical = CanonicalizeBatch(regsList);
        if (canonical.Length == 0)
        {
            IReadOnlyDictionary<string, VehicleInfoDto> empty =
                new Dictionary<string, VehicleInfoDto>(StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(empty);
        }

        var key = $"batch:{canonical}";
        return _sf.RunAsync(key, _ => _inner.GetByRegsAsync(regsList, ct), ct);
    }

    private static string Normalize(string reg) => reg.Trim().ToUpperInvariant();

    private static string CanonicalizeBatch(IEnumerable<string> regs)
    {
        var set = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var r in regs)
        {
            if (!string.IsNullOrWhiteSpace(r))
                set.Add(Normalize(r));
        }

        if (set.Count == 0) return string.Empty;

        var sb = new StringBuilder();
        var first = true;
        foreach (var s in set)
        {
            if (!first) sb.Append('|');
            sb.Append(s);
            first = false;
        }

        return sb.ToString();
    }
}
