using FluentValidation;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Unemployed;

public class PatchUnemployedDtoValidator : AbstractValidator<PatchUnemployedDto>
{
    public PatchUnemployedDtoValidator()
    {
        this.AddUnemployedPatchRules();
    }
}