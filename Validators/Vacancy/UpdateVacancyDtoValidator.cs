using FluentValidation;
using LaborExchangeApi.Dtos.VacancyDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Vacancy;

public class UpdateVacancyDtoValidator : AbstractValidator<UpdateVacancyDto>
{
    public UpdateVacancyDtoValidator()
    {
        RuleFor(v => v.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.");

        this.AddVacancyRules();
    }
}
