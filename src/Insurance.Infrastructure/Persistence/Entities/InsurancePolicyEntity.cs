namespace Insurance.Infrastructure.Persistence.Entities;

public sealed class InsurancePolicyEntity
{
    public Guid Id { get; set; }
    public string PersonalNumber { get; set; } = null!;
    public int Type { get; set; }
    public decimal MonthlyCostAmount { get; set; }
    public string MonthlyCostCurrency { get; set; } = null!;
    public string? VehicleRegNumber { get; set; }
}
