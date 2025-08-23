using Insurance.Application.Dtos;
using MediatR;

namespace Insurance.Application.UseCases.GetInsuranceSummary;

public sealed record GetInsuranceSummaryQuery(string PersonalNumber) : IRequest<InsuranceSummaryDto>;
