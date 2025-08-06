namespace LaborExchangeApi.Dtos.ResumeDtos;

public record PatchResumeDto
{
    public string? Title { get; set; }

    public string? Skills { get; set; }

    public string? Experience { get; set; }

    public int? CategoryId { get; set; }
}