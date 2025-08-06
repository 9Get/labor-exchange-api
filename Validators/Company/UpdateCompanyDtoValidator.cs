using FluentValidation;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Company;

public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
{
    public UpdateCompanyDtoValidator()
    {
        this.AddCompanyRules();
    }
}