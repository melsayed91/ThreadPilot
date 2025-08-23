namespace Insurance.Application.Dtos;

public sealed record InsuranceSummaryDto(
    string PersonalNumber,
    IReadOnlyList<PolicySummaryDto> Policies,
    decimal TotalMonthlyCost,
    string Currency);
