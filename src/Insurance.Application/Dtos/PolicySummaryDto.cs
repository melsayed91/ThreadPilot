namespace Insurance.Application.Dtos;

public sealed record PolicySummaryDto(
    string PolicyType,
    decimal MonthlyCost,
    string Currency,
    string? VehicleRegNumber,
    VehicleInfoDto? Vehicle);
