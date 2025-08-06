using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Dtos.UnemployedDtos;
using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.ResumeDtos;

public record ResumeDto(
    int Id,
    string Title,
    string Skills,
    string Experience,
    UnemployedDto Unemployed,
    CategoryDto Category
) : IResumeSharedProperties;