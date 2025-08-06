using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.ResumeDtos;

public record UpdateResumeDto(
    string Title,
    string Skills,
    string Experience,
    int CategoryId
) : IResumeSharedProperties;