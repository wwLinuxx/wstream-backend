namespace UzTube.Application.Models.Country;

public record CountryResponseModel(
    string Code,
    string FullName
) : BaseResponseModel;