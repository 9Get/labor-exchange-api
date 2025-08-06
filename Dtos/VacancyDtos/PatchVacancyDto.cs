namespace LaborExchangeApi.Dtos.VacancyDtos;

public record PatchVacancyDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal? Salary { get; set; }

    public int? CategoryId { get; set; }
}