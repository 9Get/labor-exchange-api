using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.VacancyDtos;

public record UpdateVacancyDto(
    string Title,
    string Description,
    decimal Salary,
    int CategoryId
) : IVacancySharedProperties;