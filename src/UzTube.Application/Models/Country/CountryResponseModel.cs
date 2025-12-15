namespace UzTube.Application.Models.Country;

public record CountryResponseModel : BaseResponseModel
{
    public string Code { get; init; } = null!;
    public string FullName { get; init; } = null!;
}