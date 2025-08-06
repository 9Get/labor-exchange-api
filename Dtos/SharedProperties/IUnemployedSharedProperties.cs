namespace LaborExchangeApi.Dtos.SharedProperties;

public interface IUnemployedSharedProperties
{
    string FirstName { get; }

    string LastName { get; }

    DateOnly DateOfBirth { get; }

    string ContactEmail { get; }

    string? ContactPhone { get; }
}