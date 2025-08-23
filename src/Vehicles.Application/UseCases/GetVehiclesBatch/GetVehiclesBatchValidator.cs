using FluentValidation;

namespace Vehicles.Application.UseCases.GetVehiclesBatch;

public class GetVehiclesBatchValidator : AbstractValidator<GetVehiclesBatchQuery>
{
    public GetVehiclesBatchValidator()
    {
        RuleFor(x => x.RegNumbers).NotEmpty();
        RuleForEach(x => x.RegNumbers).NotEmpty().MaximumLength(16);
    }
}
