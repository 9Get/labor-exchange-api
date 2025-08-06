using FluentValidation;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Unemployed;

public class UpdateUnemployedDtoValidator : AbstractValidator<UpdateUnemployedDto>
{
    public UpdateUnemployedDtoValidator()
    {
        this.AddUnemployedRules();
    }
}
