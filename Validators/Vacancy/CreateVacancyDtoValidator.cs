using FluentValidation;
using LaborExchangeApi.Dtos.VacancyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Vacancy;

public class CreateVacancyDtoValidator : AbstractValidator<CreateVacancyDto>
{
    public CreateVacancyDtoValidator()
    {
        RuleFor(v => v.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.");

        RuleFor(v => v.CompanyId)
            .GreaterThanOrEqualTo(0).WithMessage("Company id must be greater than 0.");

        this.AddVacancyRules();
    }
}