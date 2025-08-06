namespace LaborExchangeApi.Dtos.SharedProperties;

public interface IVacancySharedProperties
{
    string Title { get; }
    
    string Description { get; }
    
    decimal Salary { get; }
}