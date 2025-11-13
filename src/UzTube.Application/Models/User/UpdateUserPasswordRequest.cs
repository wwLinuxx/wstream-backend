namespace UzTube.Application.Models.User;

public record UpdateUserPasswordRequest(
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);

public record UpdateUserPasswordResponseModel : BaseResponseModel;