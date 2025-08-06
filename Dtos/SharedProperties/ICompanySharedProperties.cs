namespace LaborExchangeApi.Dtos.SharedProperties;

public interface ICompanySharedProperties
{
    string Name { get; }

    string? ContactPerson { get; }

    string? Email { get; }
    
    string? Phone { get; }
}