using FluentValidation;
using LaborExchangeApi.Dtos.ResumeDtos;
using LaborExchangeApi.Validators.Extensions;

namespace LaborExchangeApi.Validators.Resume;

public class CreateResumeDtoValidator : AbstractValidator<CreateResumeDto>
{
    public CreateResumeDtoValidator()
    {
        RuleFor(r => r.CategoryId)
            .GreaterThanOrEqualTo(0).WithMessage("Category id must be greater than 0.");

        RuleFor(r => r.UnemployedId)
            .GreaterThanOrEqualTo(0).WithMessage("Unemployed id must be greater than 0.");

        this.AddResumeRules();
    }
}