using FluentValidation;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Resume;

public class PatchResumeDtoValidator : AbstractValidator<PatchResumeDto>
{
    public PatchResumeDtoValidator()
    {
        RuleFor(r => r.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.");
            
        this.AddResumePatchRules();
    }
}