using FluentValidation;
using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Category;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        this.AddCategoryRules();
    }
}