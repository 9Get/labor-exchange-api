using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CompanyDtos;

public record CompanyDto(
    int Id,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
) : ICompanySharedProperties;
