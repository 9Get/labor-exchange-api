using FluentValidation;
using LaborExchangeApi.Dtos.VacancyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Vacancy;

public class PatchVacancyDtoValidator : AbstractValidator<PatchVacancyDto>
{
    public PatchVacancyDtoValidator()
    {
        RuleFor(v => v.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.")
            .When(v => v.CategoryId != null);

        this.AddVacancyPatchRules();
    }
}