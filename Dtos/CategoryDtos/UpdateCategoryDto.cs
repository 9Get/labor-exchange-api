using LaborExchangeApi.Dtos.SharedProperties;

namespace LaborExchangeApi.Dtos.CategoryDtos;

public record UpdateCategoryDto(
    string Name
) : ICategorySharedProperties;