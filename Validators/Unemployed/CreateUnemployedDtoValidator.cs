using FluentValidation;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Unemployed;

public class CreateUnemployedDtoValidator : AbstractValidator<CreateUnemployedDto>
{
    public CreateUnemployedDtoValidator()
    {
        this.AddUnemployedRules();
    }
}