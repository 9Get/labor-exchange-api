using FluentValidation;
using LaborExchangeApi.Common;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Validators.Extensions;

public static class CompanyValidatorExtensions
{
    public static void AddCompanyRules<T>(this AbstractValidator<T> validator)
        where T : ICompanySharedProperties
    {
        validator.RuleFor(c => c.Name)
            .NotNull().WithMessage("Company name is required.")
            .MaximumLength(ValidationConstants.MAX_COMPANY_NAME_LENGTH)
                .WithMessage($"Length of the company name must be less than {ValidationConstants.MAX_COMPANY_NAME_LENGTH}.");

        validator.RuleFor(c => c.Email)
            .EmailAddress().WithMessage("The email format specified is invalid.")
            .When(c => !string.IsNullOrEmpty(c.Email));

        validator.RuleFor(c => c.ContactPerson)
            .IsAValidContactPerson()
            .When(c => !string.IsNullOrEmpty(c.ContactPerson));

        validator.RuleFor(c => c.Phone).IsPhoneNumber();
    }

    public static void AddCompanyPatchRules(this AbstractValidator<PatchCompanyDto> validator)
    {
        validator.RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Company name must not be empty.")
            .MaximumLength(ValidationConstants.MAX_COMPANY_NAME_LENGTH)
                .WithMessage($"Length of the company name must be less than {ValidationConstants.MAX_COMPANY_NAME_LENGTH}.")
            .When(c => c.Name != null);

        validator.RuleFor(c => c.Email)
            .EmailAddress().WithMessage("The email format specified is invalid.")
            .When(c => c.Name != null);

        validator.RuleFor(c => c.ContactPerson)
            .IsAValidContactPerson()
            .When(c => c.Name != null);

        validator.RuleFor(c => c.Phone)
                .IsPhoneNumber().WithMessage("The phone number format specified is invalid.")
            .When(c => c.Phone != null);
    }
}