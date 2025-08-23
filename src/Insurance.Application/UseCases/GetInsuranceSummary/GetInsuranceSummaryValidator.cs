using FluentValidation;

namespace Insurance.Application.UseCases.GetInsuranceSummary;

public class GetInsuranceSummaryValidator : AbstractValidator<GetInsuranceSummaryQuery>
{
    public GetInsuranceSummaryValidator()
    {
        RuleFor(x => x.PersonalNumber)
            .NotEmpty()
            .Matches(@"^\d{8}-\d{4}$");
    }
}
