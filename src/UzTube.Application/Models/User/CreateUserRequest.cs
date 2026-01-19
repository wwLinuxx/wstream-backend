using Microsoft.AspNetCore.Http;

namespace UzTube.Application.Models.User;

public record CreateUserRequest
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public IFormFile? AvatarFile { get; init; } = null;
    public Guid CountryId { get; init; }
}

public record CreateUserResponseModel : BaseResponseModel;