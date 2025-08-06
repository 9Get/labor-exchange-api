using FluentValidation;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Resume;

public class UpdateResumeDtoValidator : AbstractValidator<UpdateResumeDto>
{
    public UpdateResumeDtoValidator()
    {
        RuleFor(r => r.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.");

        this.AddResumeRules();
    }
}
