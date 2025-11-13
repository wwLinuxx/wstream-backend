namespace UzTube.Application.Models.User;

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    int Age,
    Guid CountryId);

public record CreateUserResponseModel : BaseResponseModel;