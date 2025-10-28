namespace UzTube.Application.Models.User;

public record LoginUserModel(
    string Email,
    string Password);

public record LoginResponseModel(
    string Email,
    string Token);