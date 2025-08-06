using LaborExchangeApi.Dtos.CategoryDtos;
using LaborExchangeApi.Dtos.CompanyDtos;
using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.VacancyDtos;

public record VacancyDto(
    int Id,
    string Title,
    string Description,
    decimal Salary,
    DateTime CreatedAt,
    CompanyDto Company,
    CategoryDto Category
) : IVacancySharedProperties;