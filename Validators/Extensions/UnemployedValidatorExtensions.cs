using FluentValidation;
using LaborExchangeApi.Common;
using LaborExchangeApi.Dtos.SharedProperties;
using LaborExchangeApi.Dtos.UnemployedDtos;

namespace LaborExchangeApi.Validators.Extensions;

public static class UnemployedValidatorExtensions
{
    public static void AddUnemployedRules<T>(this AbstractValidator<T> validator)
            where T : IUnemployedSharedProperties
    {
        validator.RuleFor(u => u.FirstName)
            .NotNull().WithMessage("First name is required.")
            .MaximumLength(ValidationConstants.MAX_UNEMPLOYED_FIRST_NAME_LENGTH)
                .WithMessage($"Length of the first name must be less than {ValidationConstants.MAX_UNEMPLOYED_FIRST_NAME_LENGTH}.");

        validator.RuleFor(u => u.LastName)
            .NotNull().WithMessage("Last name is required.")
            .MaximumLength(ValidationConstants.MAX_UNEMPLOYED_LAST_NAME_LENGTH)
                    .WithMessage($"Length of the last name must be less than {ValidationConstants.MAX_UNEMPLOYED_LAST_NAME_LENGTH}.");

        validator.RuleFor(u => u.DateOfBirth)
            .IsValidDateOfBirth();

        validator.RuleFor(u => u.ContactEmail)
            .NotNull().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The email format specified is invalid.");

        validator.RuleFor(u => u.ContactPhone)
            .IsPhoneNumber()
            .When(r => !string.IsNullOrEmpty(r.ContactPhone));
    }

    public static void AddUnemployedPatchRules(this AbstractValidator<PatchUnemployedDto> validator)
    {
        validator.RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name must not be empty.")
            .MaximumLength(ValidationConstants.MAX_UNEMPLOYED_FIRST_NAME_LENGTH)
                .WithMessage($"Length of the first name must be less than {ValidationConstants.MAX_UNEMPLOYED_FIRST_NAME_LENGTH}.")
            .When(u => u.FirstName != null);

        validator.RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name must not be empty.")
            .MaximumLength(ValidationConstants.MAX_UNEMPLOYED_LAST_NAME_LENGTH)
                    .WithMessage($"Length of the last name must be less than {ValidationConstants.MAX_UNEMPLOYED_LAST_NAME_LENGTH}.")
            .When(u => u.LastName != null);

        validator.RuleFor(u => u.DateOfBirth)
            .IsValidDateOfBirth();

        validator.RuleFor(u => u.ContactEmail)
            .NotEmpty().WithMessage("Email should not be empty.")
            .EmailAddress().WithMessage("The email format specified is invalid.")
            .When(u => u.ContactEmail != null);

        validator.RuleFor(u => u.ContactPhone)
            .IsPhoneNumber().WithMessage("The phone number format specified is invalid.")
            .When(u => u.ContactPhone != null);
    }
}