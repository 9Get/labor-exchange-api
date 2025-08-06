using FluentValidation;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Company;

public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyDtoValidator()
    {
        this.AddCompanyRules();
    }    
}