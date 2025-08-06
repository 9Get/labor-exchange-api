using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.UnemployedDtos;

public record CreateUnemployedDto(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string ContactEmail,
    string? ContactPhone
) : IUnemployedSharedProperties;