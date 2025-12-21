namespace UzTube.Application.Models.User;

public record UpdateUserPasswordRequest(
    string OldPassword,
    string NewPassword);

public record UpdateUserPasswordResponseModel : BaseResponseModel;