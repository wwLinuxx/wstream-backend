namespace UzTube.Application.Models.Country;

public record CreateCountryRequest
{
    public string CountryName { get; init; } = string.Empty;
}
