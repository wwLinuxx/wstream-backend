namespace UzTube.Application.Models.User;

public record UpdateUserProfileModel(
    string FirstName,
    string LastName,
    string PhoneNumber,
    int Age,
    Guid CountryId);

public record UpdateUserProfileResponseModel : BaseResponseModel;