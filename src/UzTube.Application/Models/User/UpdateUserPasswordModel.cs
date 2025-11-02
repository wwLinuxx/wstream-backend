namespace UzTube.Application.Models.User;

public record UpdateUserPasswordModel(
    string OldPassword,
    string NewPassword,
    string ConfirmPassword);

public record UpdateUserPasswordResponseModel : BaseResponseModel;