using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.VacancyDtos;

public record CreateVacancyDto(
    string Title,
    string Description,
    decimal Salary,
    int CompanyId,
    int CategoryId
):IVacancySharedProperties;