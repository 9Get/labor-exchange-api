using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CategoryDtos;

public record CategoryDto(
    int Id,
    string Name
) : ICategorySharedProperties;
