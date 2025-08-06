using FluentValidation;
using LaborExchangeApi.Common;
using LaborExchangeApi.Dtos.SharedProperties;
using LaborExchangeApi.Dtos.VacancyDtos;

namespace LaborExchangeApi.Validators.Extensions;

public static class VacancyValidatorExtensions
{
    public static void AddVacancyRules<T>(this AbstractValidator<T> validator)
                where T : IVacancySharedProperties
    {
        validator.RuleFor(v => v.Title)
            .NotNull().WithMessage("Vacancy title is required.")
            .MaximumLength(ValidationConstants.MAX_VACANCY_TITLE_LENGTH)
                .WithMessage($"Length of the title must be less than {ValidationConstants.MAX_VACANCY_TITLE_LENGTH}.");

        validator.RuleFor(v => v.Description)
            .NotNull().WithMessage("Vacancy Description is required.")
            .MaximumLength(ValidationConstants.MAX_VACANCY_DESCRIPTION_LENGTH)
                .WithMessage($"Length of the last name must be less than {ValidationConstants.MAX_VACANCY_DESCRIPTION_LENGTH}.");

        validator.RuleFor(v => v.Salary)
            .GreaterThanOrEqualTo(0).WithMessage("Salary cannot be less than zero");
    }

    public static void AddVacancyPatchRules(this AbstractValidator<PatchVacancyDto> validator)
    {
        validator.RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Vacancy title must not be empty.")
            .MaximumLength(ValidationConstants.MAX_VACANCY_TITLE_LENGTH)
                .WithMessage($"Length of the title must be less than {ValidationConstants.MAX_VACANCY_TITLE_LENGTH}.")
            .When(v => v.Title != null);

        validator.RuleFor(v => v.Description)
            .NotNull().WithMessage("Vacancy Description must not be empty.")
            .MaximumLength(ValidationConstants.MAX_VACANCY_DESCRIPTION_LENGTH)
                .WithMessage($"Length of the last name must be less than {ValidationConstants.MAX_VACANCY_DESCRIPTION_LENGTH}.")
            .When(v => v.Title != null);

        validator.RuleFor(v => v.Salary)
            .GreaterThanOrEqualTo(0).WithMessage("Salary cannot be less than zero")
            .When(v => v.Title != null);
    }
}