using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CompanyDtos;

public record CreateCompanyDto(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
) : ICompanySharedProperties;