using FluentValidation;
using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        this.AddCategoryRules();
    }
}