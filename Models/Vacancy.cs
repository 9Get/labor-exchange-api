namespace LaborExchangeApi.Models;

public class Vacancy
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}