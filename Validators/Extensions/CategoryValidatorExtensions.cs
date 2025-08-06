using FluentValidation;
using LaborExchangeApi.Dtos.SharedProperties;
using LaborExchangeApi.Common;

namespace LaborExchangeApi.Validators.Extensions;

public static class CategoryValidatorExtensions
{
    public static void AddCategoryRules<T>(this AbstractValidator<T> validator)
        where T : ICategorySharedProperties
    {
        validator.RuleFor(c => c.Name)
            .NotNull().WithMessage("Category name is required.")
            .MaximumLength(ValidationConstants.MAX_CATEGORY_NAME_LENGTH).WithMessage($"Length of the category name must be less than {ValidationConstants.MAX_CATEGORY_NAME_LENGTH}.");
    }
}