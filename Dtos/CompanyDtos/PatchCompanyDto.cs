namespace LaborExchangeApi.Dtos.CompanyDtos;

public record PatchCompanyDto
{
    public string? Name { get; set; }

    public string? ContactPerson { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}