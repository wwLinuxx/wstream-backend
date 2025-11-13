namespace UzTube.Application.Models.User;

public record LoginUserRequest(
    string Email,
    string Password);

public record LoginResponseModel(
    string Email,
    string Token);