using FluentValidation;
using LaborExchangeApi.Common;
using PhoneNumbers;

namespace LaborExchangeApi.Validators;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string?> IsPhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(BeAValidPhoneNumber);
    }

    public static IRuleBuilderOptions<T, string?> IsAValidContactPerson<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(BeAValidContactPerson);
    }

    public static IRuleBuilderOptions<T, DateOnly> IsValidDateOfBirth<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        return ruleBuilder
            .Must((date) => IsAValidDateOfBirth(date))
                .WithMessage("Incorrect date of birth specified.");
    }

    public static IRuleBuilderOptions<T, DateOnly?> IsValidDateOfBirth<T>(this IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        return ruleBuilder
            .Must((date) =>
            {
                if (!date.HasValue) {
                    return true;
                }

                return IsAValidDateOfBirth(date.Value);
            })
                .WithMessage("Incorrect date of birth specified.");
    }

    private static bool BeAValidPhoneNumber(string? phoneNumber)
    {
        if (!string.IsNullOrEmpty(phoneNumber))
        {
            return false;
        }

        PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();

        try
        {
            PhoneNumber numberProto = phoneUtil.Parse(phoneNumber, "UA");

            return phoneUtil.IsValidNumber(numberProto);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }

    private static bool BeAValidContactPerson(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        name = name.Replace(" ", "");
        name = name.Replace("-", "");
        name = name.Replace(".", "");

        return name.All(Char.IsLetter);
    }

    private static bool IsAValidDateOfBirth(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return dateOfBirth < today &&
               dateOfBirth >= today.AddYears(-ValidationConstants.MAX_UNEMPLOYED_AGE);
    }
}