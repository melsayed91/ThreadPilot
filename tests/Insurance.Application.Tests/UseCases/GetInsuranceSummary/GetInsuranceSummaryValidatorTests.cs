using FluentAssertions;
using Insurance.Application.UseCases.GetInsuranceSummary;

namespace Insurance.Application.Tests.UseCases.GetInsuranceSummary;

public class GetInsuranceSummaryValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("19650101")]
    [InlineData("1965-0101-1234")]
    [InlineData("19650101-12")]
    public void Rejects_bad_personal_number(string pn)
    {
        var v = new GetInsuranceSummaryValidator();
        v.Validate(new GetInsuranceSummaryQuery(pn)).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("19650101-1234")]
    [InlineData("20001231-0000")]
    public void Accepts_valid_personal_number(string pn)
    {
        var v = new GetInsuranceSummaryValidator();
        v.Validate(new GetInsuranceSummaryQuery(pn)).IsValid.Should().BeTrue();
    }
}
