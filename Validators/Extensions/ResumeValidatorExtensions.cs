using FluentValidation;
using LaborExchangeApi.Common;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Validators.Extensions;

public static class ResumeValidatorExtensions
{
    public static void AddResumeRules<T>(this AbstractValidator<T> validator)
        where T : IResumeSharedProperties
    {
        validator.RuleFor(r => r.Title)
            .NotNull().WithMessage("Resume title is required.")
            .MaximumLength(ValidationConstants.MAX_RESUME_TITLE_LENGTH)
                .WithMessage($"Length of the title must be less than {ValidationConstants.MAX_RESUME_TITLE_LENGTH}.");

        validator.RuleFor(r => r.Skills)
            .NotNull().WithMessage("List of skills is required.")
            .MaximumLength(ValidationConstants.MAX_RESUME_SKILLS_LENGTH)
                .WithMessage($"Length of the skills list must be less than {ValidationConstants.MAX_RESUME_SKILLS_LENGTH}.");

        validator.RuleFor(r => r.Experience)
            .NotNull().WithMessage("Experience is required.");
    }

    public static void AddResumePatchRules(this AbstractValidator<PatchResumeDto> validator)
    {
        validator.RuleFor(r => r.Title)
            .NotEmpty().WithMessage("Resume title must not be empty.")
            .MaximumLength(ValidationConstants.MAX_RESUME_TITLE_LENGTH)
                .WithMessage($"Length of the title must be less than {ValidationConstants.MAX_RESUME_TITLE_LENGTH}.")
            .When(r => r.Title != null);

        validator.RuleFor(r => r.Skills)
            .NotEmpty().WithMessage("List of skills must not be empty.")
            .MaximumLength(ValidationConstants.MAX_RESUME_TITLE_LENGTH)
                .WithMessage($"Length of the title must be less than {ValidationConstants.MAX_RESUME_TITLE_LENGTH}.")
            .When(r => r.Title != null);

        validator.RuleFor(r => r.Experience)
            .NotEmpty().WithMessage("Experience must not be empty.")
            .When(r => r.Title != null);
    }
}