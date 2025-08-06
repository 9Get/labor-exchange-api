namespace LaborExchangeApi.Models;

public class Resume
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public int UnemployedId { get; set; }
    public Unemployed Unemployed { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}