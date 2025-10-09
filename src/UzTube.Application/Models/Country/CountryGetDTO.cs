namespace UzTube.Application.Models.Country;

public record CountryGetDTO
{
    public int Id { get; set; }

    public string Country { get; set; }
}