using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CategoryDtos;

public record CreateCategoryDto(
    string Name
) : ICategorySharedProperties;