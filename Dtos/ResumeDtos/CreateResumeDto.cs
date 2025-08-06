using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.ResumeDtos;

public record CreateResumeDto(
    string Title,
    string Skills,
    string Experience,
    int UnemployedId,
    int CategoryId
) : IResumeSharedProperties;