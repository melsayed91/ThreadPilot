using FluentValidation;

namespace Vehicles.Application.UseCases.GetVehicleByReg;

public class GetVehicleByRegValidator : AbstractValidator<GetVehicleByRegQuery>
{
    public GetVehicleByRegValidator()
    {
        RuleFor(x => x.RegNumber).NotEmpty().MaximumLength(16);
    }
}
