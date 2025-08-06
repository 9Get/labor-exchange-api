using FluentValidation;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Company;

public class PatchCompanyDtoValidator : AbstractValidator<PatchCompanyDto>
{
    public PatchCompanyDtoValidator()
    {
        this.AddCompanyPatchRules();
    }
}