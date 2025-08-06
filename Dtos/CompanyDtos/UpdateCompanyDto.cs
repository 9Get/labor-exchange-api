using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CompanyDtos;

public record UpdateCompanyDto(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
) : ICompanySharedProperties;