using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Models.User;

public record UpdateUserRequest(
    string Username,
    IFormFile? AvatarFile);

public record UpdateUserProfileResponseModel : BaseResponseModel;