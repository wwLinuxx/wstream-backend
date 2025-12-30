namespace UzTube.Application.Models.User;

public record UpdateUserRequest(
    string Username);

public record UpdateUserProfileResponseModel : BaseResponseModel;