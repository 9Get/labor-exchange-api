namespace LaborExchangeApi.Dtos.UnemployedDtos;

public record PatchUnemployedDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }
}